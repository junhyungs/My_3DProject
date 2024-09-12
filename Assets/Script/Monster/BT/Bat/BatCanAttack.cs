using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCanAttack : INode
{
    private BatBehaviour _bat;

    public BatCanAttack(BatBehaviour bat)
    {
        _bat = bat;
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

            if(distance > _bat.Agent.stoppingDistance)
            {
                return INode.State.Fail;
            }

            _bat.Agent.SetDestination(_bat.transform.position);

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
