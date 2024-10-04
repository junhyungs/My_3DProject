using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mother_Hyper : Mother, IMotherPattern
{
    private Transform _playerTransform;
    private CapsuleCollider _motherCollider;

    private Vector3 _movePosition;
    private Vector3 _myPosition;

    private int _collidingCount;

    public void InitializeOnAwake(ForestMother mother, ForestMotherProperty property)
    {
        _mother = mother;
        _property = property;
        _animator = mother.GetComponent<Animator>();
        _agent = mother.GetComponent<NavMeshAgent>();
        _motherCollider = mother.GetComponent<CapsuleCollider>();   
        
        _agent.speed = _property.CurrentSpeed;
        _myPosition = mother.transform.position;    
    }

    public void OnStart()
    {
        _property.IsPlaying = true; 

        _playerTransform = _property.PlayerObject.transform;

        _motherCollider.isTrigger = true;        

        _animator.SetTrigger(_hyperTrigger);

        _movePosition = MoveToPlayer(_playerTransform);

        _agent.SetDestination(_movePosition);
    }

    public void OnUpdate()
    {
        if(_collidingCount >= 2)
        {
            _agent.SetDestination(_myPosition);

            float distance = Vector3.Distance(_mother.transform.position, _myPosition);

            if (distance <= 0.1f)
            {
                _animator.SetBool(_hyperBool, false);

                _property.IsPlaying = false;
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
        _collidingCount = 0;

        _motherCollider.isTrigger = false;
    }

    public void Colliding(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if(hit != null)
            {
                hit.TakeDamage(_property.CurrentPower);
            }
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _collidingCount++;

            _movePosition = MoveToPlayer(_playerTransform);
        }
    }

    private Vector3 MoveToPlayer(Transform playerTransform)
    {
        Vector3 rayDirection = (playerTransform.position - _mother.transform.position).normalized;

        Vector3 rayPosition = _mother.transform.position +
            _mother.transform.TransformDirection(new Vector3(0f, 0.5f, 0f)) + _mother.transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(rayPosition, rayDirection, 20f, LayerMask.GetMask("Wall"));

        if(hits.Length > 0)
        {
            float backDistance = 2.5f;

            Vector3 movePosition = hits[0].point - rayDirection * backDistance;

            return movePosition;
        }

        Debug.Log("레이가 감지하지 못함");
        return playerTransform.position;
    }
}
