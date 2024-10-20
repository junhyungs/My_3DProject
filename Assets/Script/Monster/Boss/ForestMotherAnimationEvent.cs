using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MotherVine
{
    [Header("VineType")]
    public VineType _vineType;
    [Header("VineCollider")]
    public GameObject[] _colliderArray;

    [HideInInspector]
    public SphereCollider[] _sphereColliders;
    [HideInInspector]
    public HashSet<int> _overlapHashSet;
}

public class ForestMotherAnimationEvent : MonoBehaviour
{
    [Header("ShootTransform")]
    [SerializeField] private Transform _shootTransform;
    [Header("ColliderClass")]
    [SerializeField]private List<MotherVine> _motherVineList;

    private Animator _animator;
    private ForestMotherData _data;
    private Dictionary<VineType, List<MotherVine>> _vineColliderDictionary;
    private List<MotherVine> _onList = new List<MotherVine>();  

    private readonly int _slam = Animator.StringToHash("Slam");
    private readonly int _slamRotation = Animator.StringToHash("SlamRotation");
    private readonly int _spinIdle = Animator.StringToHash("SpinIdle");
    private readonly int _hyper = Animator.StringToHash("Hyper");

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _data = BossManager.Instance.MotherData;

        InitializeCollider();
    }

    #region SpinIdle
    //SpinIdle => Hyper
    public void StartSpinIdle() => _animator.SetBool(_spinIdle, true);
    public void StopSpinIdle() => _animator.SetBool(_spinIdle, false);
    public void ActiveSpinIdle(bool isActive) => _animator.SetBool(_spinIdle, isActive);
    #endregion

    #region Hyper
    //Hyper 시작 -> HyperSpin(Trigger) Hyper 종료 -> Hyper(SetBool(false))
    public void StartHyper() => _animator.SetBool(_hyper, true);
    public void StopHyper() => _animator.SetBool(_hyper, false);    
    #endregion

    #region Lift
    public void UpperWeightZero() => _animator.SetLayerWeight(1, 0f);
    public void UpperWeight() => _animator.SetLayerWeight(1, 1f);
    #endregion

    #region Slam
    //Slam 시작 -> SlamSpin(Trigger)
    public void StartSlamRotation() => _animator.SetTrigger(_slamRotation);
    public void StartSlam() => _animator.SetTrigger(_slam);
    #endregion

    #region Shoot
    public void Shoot()
    {
        GameObject projectile = ObjectPool.Instance.DequeueObject(ObjectName.MotherProjectile);

        projectile.transform.position = _shootTransform.position;
        projectile.transform.rotation = _shootTransform.rotation;

        GameObject fireParticleObject = projectile.transform.GetChild(1).gameObject;
        ParticleSystem fireParticle =  fireParticleObject.GetComponent<ParticleSystem>();
        fireParticle.Play();

        MotherProjectile projectileComponent = projectile.GetComponent<MotherProjectile>();
        Transform player = GameManager.Instance.Player.transform;
        projectileComponent.PlayerTransform = player;
        projectileComponent.ProjectileMove();
    }
    #endregion

    private void InitializeCollider()
    {
        _vineColliderDictionary = new Dictionary<VineType, List<MotherVine>>();

        foreach(var motherVine in _motherVineList)
        {
            motherVine._sphereColliders = new SphereCollider[motherVine._colliderArray.Length];
            motherVine._overlapHashSet = new HashSet<int>();

            for(int i = 0; i < motherVine._colliderArray.Length; i++)
            {
                GameObject colliderObject = motherVine._colliderArray[i];

                if(colliderObject != null)
                {
                    AddComponents(motherVine._sphereColliders,
                        colliderObject, i, motherVine);
                }
            }

            switch(motherVine._vineType)
            {
                case VineType.Right:
                    AddDictionary(motherVine._vineType, motherVine);
                    break;
                case VineType.Left:
                    AddDictionary(motherVine._vineType, motherVine);
                    break;
            }
        }
    }

    private void AddDictionary(VineType vineType, MotherVine motherVine)
    {
        if (!_vineColliderDictionary.ContainsKey(vineType))
        {
            _vineColliderDictionary[vineType] = new List<MotherVine>();
        }

        _vineColliderDictionary[vineType].Add(motherVine);
    }

    private void AddComponents(SphereCollider[] sphereColliders ,GameObject colliderObject, int index, MotherVine motherVine)
    {
        colliderObject.AddComponent<SphereCollider>();

        SphereCollider collider = colliderObject.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 0.5f;
        collider.enabled = false;

        colliderObject.AddComponent<Mother_VineHandler>().Initialize(motherVine, index, _data.Power);
        sphereColliders[index] = collider;
    }

    private List<MotherVine> GetMotherVineList(VineType key)
    {
        if(_vineColliderDictionary.TryGetValue(key, out List<MotherVine> list))
        {
            return list;
        }

        return null;
    }

    public void OnRightCollider()
    {
        List<MotherVine> vineList = GetMotherVineList(VineType.Right);

        foreach(var motherVine in vineList)
        {
            for(int i = 0; i < motherVine._sphereColliders.Length; i++)
            {
                motherVine._sphereColliders[i].enabled = true;
            }

            _onList.Add(motherVine);
        }
    }

    public void OnLeftCollider()
    {
        List<MotherVine> vineList = GetMotherVineList(VineType.Left);

        foreach(var motherVine in vineList)
        {
            for(int i = 0; i < motherVine._sphereColliders.Length; i++)
            {
                motherVine._sphereColliders[i].enabled = true;
            }

            _onList.Add(motherVine);
        }
    }

    public void OffRightCollider()
    {
        foreach(var onMotherVine in _onList)
        {
            for(int i = 0; i < onMotherVine._sphereColliders.Length; i++)
            {
                onMotherVine._sphereColliders[i].enabled = false;
            }

            onMotherVine._overlapHashSet.Clear();
        }

        _onList.Clear();
    }

    public void OffLeftCollider()
    {
        foreach (var onMotherVine in _onList)
        {
            for (int i = 0; i < onMotherVine._sphereColliders.Length; i++)
            {
                onMotherVine._sphereColliders[i].enabled = false;
            }

            onMotherVine._overlapHashSet.Clear();
        }

        _onList.Clear();
    }

    public void OnAllCollider()
    {
        foreach(var allList in _motherVineList)
        {
            for(int i = 0; i <  allList._sphereColliders.Length; i++)
            {
                allList._sphereColliders[i].enabled = true;
            }
        }
    }

    public void OffAllCollider()
    {
        foreach (var allList in _motherVineList)
        {
            for (int i = 0; i <  allList._sphereColliders.Length; i++)
            {
                allList._sphereColliders[i].enabled = false;
            }

            allList._overlapHashSet.Clear();
        }
    }
}
