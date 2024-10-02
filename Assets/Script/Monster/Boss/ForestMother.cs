using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ForestMother : MonoBehaviour, IDamged
{
    [Header("SoulPosition")]
    [SerializeField] private Transform _soulTransform;
    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderer;
    [Header("LowerBodyMesh")]
    [SerializeField] private MeshRenderer _lowerBody;
    [Header("Material")]
    [SerializeField] private Material _material;

    #region Component
    private Material _copyMaterial;
    private WaitForSeconds _intensityTime = new WaitForSeconds(0.1f);
    private ForestMotherProperty _property;
    private Animator _animator;
    public ForestMotherProperty Property => _property;
    #endregion

    #region Value
    private float _currentHp;
    private float _currentSpeed;
    private float _currentPower;
    #endregion

    private void Start()
    {
        InitializeForestMother();
        InitializeData();
        InitializeProperty();
    }

    private void InitializeForestMother()
    {
        _animator = gameObject.GetComponent<Animator>();

        SetMaterial();
    }

    private void InitializeData()
    {
        var data = BossManager.Instance.MotherData;

        _currentHp = data.Health;
        _currentSpeed = data.Speed;
        _currentPower = data.Power;
    }

    private void InitializeProperty()
    {
        _property = new ForestMotherProperty();

        _property.CurrentHP = _currentHp;
        _property.CurrentSpeed = _currentSpeed;
        _property.CurrentPower = _currentPower;
        _property.SoulTransform = _soulTransform;
    }

    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_material);

        _lowerBody.material = _copyMaterial;

        foreach (var newMaterial in _skinnedMeshRenderer)
        {
            newMaterial.material = _copyMaterial;
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        SkillManager.Instance.SkillCount++;

        if (_currentHp > 0)
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }

    private IEnumerator IntensityChange(float baseValue, float power)
    {
        Color currentColor = _copyMaterial.GetColor("_Color");

        Color intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _copyMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _copyMaterial.SetColor("_Color", currentColor);
    }

    public void Die(Transform soulTransform)
    {
        MonsterSoul(soulTransform);

        this.gameObject.layer = LayerMask.NameToLayer("DeadMonster");

        _animator.SetTrigger("Die");

        StartCoroutine(FireShader(5f, 0.5f, -0.3f));
    }

    private IEnumerator FireShader(float maxTime, float startValue, float endValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;

            float colorValue = Mathf.Lerp(startValue, endValue, elapsedTime / maxTime);

            _copyMaterial.SetFloat("_Float", colorValue);

            yield return null;
        }
    }

    private void MonsterSoul(Transform soulTransform)
    {
        GameObject soul = ObjectPool.Instance.DequeueObject(ObjectName.Soul);
        DropSoul component = soul.GetComponent<DropSoul>();
        soul.transform.position = soulTransform.position;
        soul.SetActive(true);
        component.StartCoroutine(component.Fly());
    }
}
