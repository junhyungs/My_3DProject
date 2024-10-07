using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Mother_Lift : Mother, IMotherPattern
{
    private CapsuleCollider _motherCollider;
    private ForestMotherVine[] _vine;

    private float _currentLiftTime;
    private float _startLiftTime;
    private float _maxLiftTime;
    
    private bool _startLift;
    private bool _clear;
    private bool _canShoot;

    private Vine _selectVine;

    public void InitializeOnAwake(ForestMother mother, ForestMotherProperty property)
    {
        _mother = mother;
        _property = property;
        _animator = mother.GetComponent<Animator>();
        _motherCollider = mother.GetComponent<CapsuleCollider>();
        _motherRig = mother.transform.GetComponentInChildren<Rig>();
        _vine = mother.transform.GetComponentsInChildren<ForestMotherVine>();

        _maxLiftTime = 20f;
        _canShoot = true;
    }

    

    public void OnStart()
    {
        BossManager.Instance.AddVineEvent(ReceiveVine, true);

        SaveVine();

        ChangeTriggerCollider(true);

        _motherRig.weight = 1f;

        _property.IsPlaying = true;

        _animator.SetTrigger(_liftTrigger);

        _mother.StartCoroutine(LiftDelay());
    }

    public void OnUpdate()
    {
        if (_startLift && !_clear)
        {
            _currentLiftTime = Time.time;

            if (_currentLiftTime - _startLiftTime > _maxLiftTime)
            {
                _animator.SetTrigger(_liftFailTrigger);

                _startLift = false;

                _mother.StartCoroutine(ChangeDelay());
            }

            MotherShoot();
        }
    }

    public bool IsRunning()
    {
        bool isPlaying = _property.IsPlaying;

        if (isPlaying)
        {
            return true;
        }

        return false;
    }

    public void OnEnd()
    {
        ChangeTriggerCollider(false);
        
        _motherRig.weight = 0f;

        _startLift = false;

        _canShoot = true;

        _clear = false;

        BossManager.Instance.AddVineEvent(ReceiveVine, false);
    }

    private IEnumerator LiftDelay()
    {
        yield return new WaitUntil(() =>
        {
            int layerIndex = _animator.GetLayerIndex("BaseLayer");

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex);

            return stateInfo.IsName("LiftIdle");
        });

        _startLift = true;

        _startLiftTime = Time.time;
    }

    private void MotherShoot()
    {
        if(_canShoot)
        {
            _animator.SetTrigger(_shootTrigger);

            _mother.StartCoroutine(CanShoot());
        }
    }

    private IEnumerator CanShoot()
    {
        _canShoot = false;

        yield return new WaitForSeconds(8f);

        _canShoot = true;
    }

    public void ReceiveVine(Vine vineType, float currentHealth)
    {
        if(_selectVine != Vine.Null && _selectVine != vineType)
        {
            return;
        }

        if(currentHealth == _property.DownHealth)
        {
            _clear = true;

            LiftDamageAnimation(vineType, true);
        }

        if(currentHealth <= 0)
        {
            LiftDamageAnimation(vineType, false);

            _mother.StartCoroutine(ChangeDelay());
        }
    }

    private IEnumerator ChangeDelay()
    {
        yield return new WaitForSeconds(2.0f);

        _property.IsPlaying = false;

    }

    private void LiftDamageAnimation(Vine vineType, bool isActive)
    {
        if (vineType is Vine.Right)
        {
            _animator.SetBool(_liftDamageR_Bool, isActive);
        }
        else if (vineType is Vine.Left)
        {
            _animator.SetBool(_liftDamageL_Bool, isActive);
        }

        _selectVine = vineType;
    }

    private void ChangeTriggerCollider(bool change)
    {
        foreach(var vine in _vine)
        {
            vine.EnabledCollider(change);
        }

        _motherCollider.isTrigger = change;

        _mother.gameObject.layer = change ? LayerMask.NameToLayer("Lift") : LayerMask.NameToLayer("Monster");
    }
    private void SaveVine()
    {
        _selectVine = Vine.Null;

        foreach (var vine in _vine)
        {
            vine.SaveMaterial();
        }
    }
}
