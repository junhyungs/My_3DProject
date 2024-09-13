using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatMoveToPlayer : INode
{
    private BatBehaviour _bat;
    private NavMeshAgent _agent;

    public BatMoveToPlayer(BatBehaviour bat, NavMeshAgent agent)
    {
        _bat = bat;
        _agent = agent;
    }

    public INode.State Evaluate()
    {
        if (!_bat.CheckPlayer)
        {
            return INode.State.Fail;
        }

        if (_bat.IsAttack)
        {
            return INode.State.Fail;
        }

        Transform playerTransform = _bat.PlayerObject.transform;

        _agent.SetDestination(playerTransform.position);

        return INode.State.Running;
    }
}
