using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DekuReturn : INode
{
    private DekuBehaviour _deku;
    private NavMeshAgent _agent;
    private Animator _animator;

    private Vector3 _startPosition;

    public DekuReturn(DekuBehaviour deku)
    {
        _deku = deku;
        _agent = _deku.GetComponent<NavMeshAgent>();    
        _animator = _deku.GetComponent<Animator>();

        _startPosition = _deku.transform.position;
    }

    public INode.State Evaluate()
    {
        float distance = Vector3.Distance(_startPosition, _deku.transform.position);

        if(distance > 0.1f)
        {
            _animator.SetBool("Walk", true);

            _agent.SetDestination(_startPosition);

            return INode.State.Running;
        }

        _animator.SetBool("Walk", false);

        _agent.SetDestination(_deku.transform.position);

        _deku.IsReturn = false;

        _agent.stoppingDistance = 5f;

        return INode.State.Success;
    }
}
