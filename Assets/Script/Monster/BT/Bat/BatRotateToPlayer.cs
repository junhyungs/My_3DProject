using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRotateToPlayer : INode
{
    private BatBehaviour _bat;
    private float _rotationSpeed;

    public BatRotateToPlayer(BatBehaviour bat)
    {
        _bat = bat;
        _rotationSpeed = 20f;
    }

    public INode.State Evaluate()
    {
        Transform playerTransform = _bat.PlayerObject.transform;

        Vector3 rotateDirection = (playerTransform.position - _bat.transform.position).normalized;

        rotateDirection.y = 0f;

        float angle = Vector3.Angle(_bat.transform.forward, rotateDirection);

        if(angle > 2f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotateDirection);

            _bat.transform.rotation = Quaternion.Slerp(_bat.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            return INode.State.Running;
        }

        return INode.State.Success;
    }
}
