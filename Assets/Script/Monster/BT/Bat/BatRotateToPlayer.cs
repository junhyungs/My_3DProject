using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRotateToPlayer : INode
{
    private BatBehaviour _bat;
    private float _rotationSpeed;
    private float _angle;

    private Quaternion _rotation;

    public BatRotateToPlayer(BatBehaviour bat)
    {
        _bat = bat;
        _rotationSpeed = 20f;
    }

    public INode.State Evaluate()
    {
        Transform playerTransform = _bat.PlayerObject.transform;

        Vector3 rotateDirection = (playerTransform.position - _bat.transform.position).normalized;

        if(rotateDirection == Vector3.zero)
        {
            return INode.State.Success;
        }

        rotateDirection.y = 0f;

        _angle = Vector3.Angle(_bat.transform.forward, rotateDirection);

        if(_angle > 10f)
        {
            _rotation = Quaternion.LookRotation(rotateDirection);

            _bat.transform.rotation = Quaternion.Slerp(_bat.transform.rotation, _rotation, _rotationSpeed * Time.deltaTime);

            return INode.State.Running;
        }

        _bat.transform.rotation = Quaternion.LookRotation(rotateDirection);

        return INode.State.Success;
    }
}
