using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageRotateToPlayer : INode
{
    private MageBehaviour _mage;

    private float _rotationSpeed;
    private float _angle;

    public MageRotateToPlayer(MageBehaviour mageBehaviour)
    {
        _mage = mageBehaviour;

        _rotationSpeed = 20f;
    }

    public INode.State Evaluate()
    {
        if(_mage.AttackCount > 0)
        {
            return INode.State.Success;
        }

        Transform playerTransform = _mage.PlayerObject.transform;

        Vector3 targetDirection = (playerTransform.position - _mage.transform.position).normalized;

        targetDirection.y = 0f;

        Quaternion rotation = Quaternion.LookRotation(targetDirection);

        _mage.transform.rotation = Quaternion.Slerp(_mage.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

        _angle = Vector3.Angle(_mage.transform.forward, targetDirection);

        if(_angle < 3f)
        {
            return INode.State.Success;
        }

        return INode.State.Running;
    }
}
