using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulMoveToPlayer : INode
{
    private GhoulBehaviour _ghoul;
    private NavMeshAgent _agent;
    private Animator _animator;

    private float _returnDistance;
    private float _stoppingDistance;
    private bool _isReturn;
    private Vector3 _startPosition;

    public GhoulMoveToPlayer(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;
        _agent = _ghoul.GetComponent<NavMeshAgent>();
        _animator = _ghoul.GetComponent<Animator>();

        _returnDistance = 15f;
        _stoppingDistance = _agent.stoppingDistance;
        _startPosition = _ghoul.transform.position;
    }

    public INode.State Evaluate()
    {
        if(!_ghoul.CheckPlayer)
        {
            return INode.State.Fail;
        }

        _animator.SetBool("TraceWalk", true);

        if (_ghoul.IsReturn)
        {
            _agent.stoppingDistance = 0f;

            _agent.SetDestination(_startPosition);

            if(_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.stoppingDistance = _stoppingDistance;

                _agent.isStopped = true;

                _ghoul.CheckPlayer = false;

                _animator.SetBool("TraceWalk", false);

                return INode.State.Success;
            }

            return INode.State.Running;
        }

        Transform playerTransform = _ghoul.PlayerObject.transform;

        _agent.SetDestination(playerTransform.position);

        return INode.State.Running;
    }
}
