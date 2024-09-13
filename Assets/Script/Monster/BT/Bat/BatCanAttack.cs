using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatCanAttack : INode
{
    private BatBehaviour _bat;
    private NavMeshAgent _agent;

    public BatCanAttack(BatBehaviour bat, NavMeshAgent agent)
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

        if (!_bat.IsAttack)
        {
            Transform playerTransform = _bat.PlayerObject.transform;

            float distance = Vector3.Distance(playerTransform.position, _bat.transform.position);

            if(distance > _agent.stoppingDistance)
            {
                return INode.State.Fail;
            }

            _agent.SetDestination(_bat.transform.position);

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
