using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_Lift : Mother, IMotherPattern
{
    private float _currentTime;
    private float _startTime;
    private float _maxTime;
    private float _overlapRadius;
    private float _startShootTime;
    private float _shootTime;
    private float _currentShootTime;

    private bool _startLift;

    public void InitializeOnAwake(ForestMother mother, ForestMotherProperty property)
    {
        _mother = mother;
        _property = property;
        _animator = mother.GetComponent<Animator>();

        _maxTime = 20f;
        _shootTime = 3f;
    }

    public void OnStart()
    {
        BossManager.Instance.AddVineEvent(ReceiveVine, true);

        _property.IsPlaying = true;

        _animator.SetTrigger(_liftTrigger);

        _mother.StartCoroutine(LiftDelay());
    }

    public void OnUpdate()
    {
        if (_startLift)
        {
            _currentTime = Time.time;

            if (_currentTime - _startTime > _maxTime)
            {
                
                Collider[] colliders = Physics.OverlapSphere(_mother.transform.position, _overlapRadius,
                    LayerMask.GetMask("Player"));

                if(colliders.Length > 0)
                {
                    IDamged hit = colliders[0].gameObject.GetComponent<IDamged>();

                    hit.TakeDamage(3f);
                }
                
                _animator.SetTrigger(_liftFailTrigger);

                _mother.StartCoroutine(ChangeDelay());

                _startLift = false;
            }
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
        _startLift = false;

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

        _startTime = Time.time;

        _mother.StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        _startShootTime = Time.time;

        yield return new WaitWhile(() =>
        {
            _currentShootTime = Time.time;

            int layerIndex = _animator.GetLayerIndex("BaseLayer");

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex);

            if(_currentShootTime - _startShootTime > _shootTime)
            {
                _animator.SetTrigger(_shootTrigger);
            }

            return stateInfo.IsName("LiftIdle");
        });

        _animator.ResetTrigger(_shootTrigger);

        _startShootTime = 0f;
        _currentShootTime = 0f;
    }

    public void ReceiveVine(Vine vineType, float currentHealth)
    {
        Debug.Log(vineType);
        Debug.Log(currentHealth);
        if(currentHealth == _property.DownHealth)
        {
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
    }
}
