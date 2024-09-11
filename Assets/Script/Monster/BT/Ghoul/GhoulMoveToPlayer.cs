using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulMoveToPlayer : INode
{
    private GhoulBehaviour _ghoul;

    private float _returnDistance;
    private float _stoppingDistance;
    private bool _isReturn;
    private Vector3 _startPosition;

    public GhoulMoveToPlayer(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;

        _returnDistance = 15f;
        _stoppingDistance = _ghoul.Agent.stoppingDistance;
        _startPosition = _ghoul.transform.position;
    }

    public INode.State Evaluate()
    {
        if (!_ghoul.CheckPlayer)
        {
            return INode.State.Fail;
        }

        if (!_ghoul.CanMove)
        {
            return INode.State.Success;
        }

        Transform playerTransform = _ghoul.PlayerObject.transform;

        float currentDistance = Vector3.Distance(_ghoul.transform.position, playerTransform.position);

        if (!_isReturn)
        {
            if(currentDistance > _returnDistance)
            {
                _isReturn = true;

                _ghoul.Agent.stoppingDistance = 0f;

                return INode.State.Fail;
            }

            if(currentDistance > _ghoul.Agent.stoppingDistance)
            {
                _ghoul.Animator.SetBool("TraceWalk", true);

                _ghoul.Agent.SetDestination(playerTransform.position);

                return INode.State.Running;
            }

            _ghoul.Animator.SetBool("TraceWalk", false);

            _ghoul.Agent.SetDestination(_ghoul.transform.position);

            return INode.State.Success;
        }
        else
        {
            if(currentDistance < _stoppingDistance)
            {
                _isReturn = false;

                _ghoul.Agent.stoppingDistance = _stoppingDistance;

                _ghoul.Animator.SetBool("TraceWalk", false);

                _ghoul.Agent.SetDestination(_ghoul.transform.position);

                return INode.State.Success;
            }

            _ghoul.Animator.SetBool("TraceWalk", true);

            _ghoul.Agent.SetDestination(_startPosition);

            if(_ghoul.Agent.remainingDistance <= _ghoul.Agent.stoppingDistance)
            {
                _isReturn = false;

                _ghoul.CheckPlayer = false;

                _ghoul.Animator.SetBool("TraceWalk", false);

                _ghoul.Agent.stoppingDistance = _stoppingDistance;

                return INode.State.Fail;    
            }

            return INode.State.Running;
        }
    }

}
