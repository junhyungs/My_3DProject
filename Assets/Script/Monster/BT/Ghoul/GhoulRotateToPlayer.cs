using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulRotateToPlayer : INode
{
    private GhoulBehaviour _ghoul;
    private float _rotationSpeed;
    private float _angle;

    private Quaternion _rotation;

    public GhoulRotateToPlayer(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;

        _rotationSpeed = 20f;
    }

    public INode.State Evaluate()
    {
        Transform playerTransform = _ghoul.PlayerObject.transform;

        Vector3 rotateDirection = (playerTransform.position - _ghoul.transform.position).normalized;

        _angle = Vector3.Angle(_ghoul.transform.forward, rotateDirection);

        if(_angle > 2f)
        {
            _rotation = Quaternion.LookRotation(rotateDirection);

            _ghoul.transform.rotation = Quaternion.Slerp(_ghoul.transform.rotation,
                _rotation, _rotationSpeed * Time.deltaTime);

            return INode.State.Running;
        }

        _ghoul.transform.rotation = Quaternion.LookRotation(rotateDirection);

        return INode.State.Success;
    }
}
