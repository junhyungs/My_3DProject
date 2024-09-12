using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMoveToPlayer : INode
{
    private BatBehaviour _bat;

    public BatMoveToPlayer(BatBehaviour bat)
    {
        _bat = bat;
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

        _bat.Agent.SetDestination(playerTransform.position);

        return INode.State.Running;
    }
}
