using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatMoveToPlayer : INode
{
    private BatBehaviour _bat;
    private NavMeshAgent _agent;

    private Vector3 _startPosition;

    public BatMoveToPlayer(BatBehaviour bat)
    {
        _bat = bat;
        _agent = _bat.GetComponent<NavMeshAgent>();

        _startPosition = _bat.transform.position;
    }

    public INode.State Evaluate()
    {
        if (!_bat.CheckPlayer)
        {
            return INode.State.Fail;
        }

        if (_bat.IsReturn)
        {
            _agent.stoppingDistance = 0f;

            _agent.SetDestination(_startPosition);

            if(_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.SetDestination(_bat.transform.position);

                _bat.IsReturn = false;

                _bat.CheckPlayer = false;

                return INode.State.Success;
            }

            return INode.State.Running;
        }

        Transform playerTransform = _bat.PlayerObject.transform;

        _agent.SetDestination(playerTransform.position);

        return INode.State.Running;
    }
}
