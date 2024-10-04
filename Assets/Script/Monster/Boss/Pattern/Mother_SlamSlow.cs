using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Mother_SlamSlow : Mother, IMotherPattern
{
    private Rig _motherRig;
    private Transform _playerTransform;
    private WaitForSeconds _rotateDelayTime;

    private Vector3 _rotateDirection;
    private Vector3 _directionToPlayer;

    private float _currentAngle;
    private float _startTime;
    private float _currentTime;
    private float _maxRotationTime = 5f;
    private float _rotationSpeed = 30f;

    private bool _rotation;

    public void InitializeOnAwake(ForestMother mother, ForestMotherProperty property)
    {
        _mother = mother;
        _property = property;
        _animator = mother.GetComponent<Animator>();
        _motherRig = mother.transform.GetComponentInChildren<Rig>();
        _rotateDelayTime = new WaitForSeconds(0.5f);
    }

    public void OnStart()
    {
        _property.IsPlaying = true;

        _motherRig.weight = 0f;

        _animator.SetTrigger(_slamSlowTrigger);

        _animator.SetBool(_slamSlowBool, true);

        RotateDirection();
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

    public void OnUpdate()
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
                _animator.SetBool(_slamSlowBool, false);
            }
        }
    }

    public void OnEnd()
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

        yield return _rotateDelayTime;

        _rotation = true;
    }

    private void RotateDirection()
    {
        _playerTransform = _property.PlayerObject.transform;

        _directionToPlayer = _playerTransform.position - _mother.transform.position;

        _currentAngle = Vector3.SignedAngle(_mother.transform.forward, _directionToPlayer, Vector3.up);

        if (_currentAngle > 0)
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

}
