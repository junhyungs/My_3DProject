using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageMoveToPlayer : INode
{
    private MageBehaviour _mage;

    public MageMoveToPlayer(MageBehaviour mageBehaviour)
    {
        _mage = mageBehaviour;
    }

    public INode.State Evaluate()
    {
        if (!_mage.CheckPlayer)
        {
            return INode.State.Fail;
        }

        if (!_mage.CanMove)
        {
            return INode.State.Fail;
        }

        Transform playerTransform = _mage.PlayerObject.transform;

        float currentDistance = Vector3.Distance(playerTransform.position, _mage.transform.position);

        if(currentDistance > _mage.Agent.stoppingDistance)
        {
            _mage.Agent.SetDestination(playerTransform.position);
        }

        return INode.State.Success;
    }
}
