using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulRotateToPlayer : INode
{
    private GhoulBehaviour _ghoul;
    private float _rotationSpeed;
    private float _angle;

    public GhoulRotateToPlayer(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;

        _rotationSpeed = 20f;
    }

    public INode.State Evaluate()
    {
        if (!_ghoul.CanRotation)
        {
            return INode.State.Fail;
        }

        Transform playerTransform = _ghoul.PlayerObject.transform;

        Vector3 rotateDirection = (playerTransform.position - _ghoul.transform.position).normalized;

        rotateDirection.y = 0f;

        _angle = Vector3.Angle(_ghoul.transform.forward, rotateDirection);

        if(_angle >= 1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotateDirection);

            _ghoul.transform.rotation = Quaternion.Slerp(_ghoul.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            return INode.State.Running;
        }

        return INode.State.Success;
    }
}
