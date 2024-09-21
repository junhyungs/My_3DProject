using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DekuMove : INode
{
    private DekuBehaviour _deku;
    private NavMeshAgent _agent;
    private Animator _animator;

    private Vector3 _startPosition;

    public DekuMove(DekuBehaviour deku)
    {
        _deku = deku;
        _agent = _deku.GetComponent<NavMeshAgent>();
        _animator = _deku.GetComponent<Animator>();

        _startPosition = _deku.transform.position;
    }

    public INode.State Evaluate()
    {
        if (!_deku.CheckPlayer)
        {
            return INode.State.Fail;
        }

        _animator.SetBool("Walk", true);

        if (_deku.IsReturn)
        {
            _agent.stoppingDistance = 0f;

            _agent.SetDestination(_startPosition);

            if(_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.SetDestination(_deku.transform.position);

                _deku.CheckPlayer = false;

                _animator.SetBool("Walk", false);

                _deku.IsReturn = false;

                return INode.State.Success;
            }

            return INode.State.Running;
        }

        Transform playerTransform = _deku.PlayerObject.transform;

        _agent.SetDestination(playerTransform.position);

        return INode.State.Running;
    }
}
