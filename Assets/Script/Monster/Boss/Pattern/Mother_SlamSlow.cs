using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Mother_SlamSlow : IMotherPattern
{
    private ForestMother _mother;
    private ForestMotherProperty _property;
    private Animator _animator;

    private Rig _motherRig;

    private Vector3 _rotateDirection;

    private readonly int _slamSlow = Animator.StringToHash("SlamSlow");
    private readonly int _startSlow = Animator.StringToHash("Slow");

    private float _startTime;
    private float _currentTime;
    private float _maxRotationTime = 5f;
    private float _rotationSpeed = 30f;

    private bool _rotation;

    public void InitializePattern(ForestMother mother)
    {
        Debug.Log("SlamSlow √ ±‚»≠");
        if(_mother == null)
        {
            _mother = mother;
            _property = _mother.Property;

            _motherRig = _mother.transform.GetComponentInChildren<Rig>();
            _animator = _mother.GetComponent<Animator>();
        }

        _property.IsPlaying = true;

        _motherRig.weight = 0f;

        _animator.SetTrigger(_slamSlow);

        _animator.SetBool(_startSlow, true);

        Transform playerTransform = _property.PlayerObject.transform;

        Vector3 direction = playerTransform.position - _mother.transform.position;

        float angle = Vector3.SignedAngle(_mother.transform.forward, direction, Vector3.up);

        if(angle > 0)
        {
            _rotateDirection = Vector3.up;
        }
        else
        {
            _rotateDirection = Vector3.down;
        }

        _startTime = Time.time;

        _mother.StartCoroutine(RotationDelay());
    }

    public bool IsPlay()
    {
        bool isPlaying = _property.IsPlaying;
        
        if (isPlaying)
        {
            return true;
        }

        return false;
    }

    public void PlayPattern()
    {
        if (_rotation)
        {
            _currentTime = Time.time;

            if (_currentTime - _startTime <= _maxRotationTime)
            {
                Quaternion rotation = Quaternion.Euler(_rotateDirection * _rotationSpeed * Time.deltaTime);

                _mother.transform.rotation = Quaternion.RotateTowards(_mother.transform.rotation,
                    _mother.transform.rotation * rotation, _rotationSpeed * Time.deltaTime);
            }
            else
            {
                _animator.SetBool(_startSlow, false);
            }
        }
    }

    public void EndPattern()
    {
        _startTime = 0f;

        _currentTime = 0f;

        _motherRig.weight = 1f;

        _rotation = false;
    }

    private IEnumerator RotationDelay()
    {
        yield return new WaitUntil(() =>
        {
            int layerIndex = _animator.GetLayerIndex("LowerLayer");
            
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex);

            return stateInfo.IsName("Slam_slow_idle");
        });

        yield return new WaitForSeconds(0.5f);

        _rotation = true;
    }

}
