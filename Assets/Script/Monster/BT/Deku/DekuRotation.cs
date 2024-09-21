using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DekuRotation : INode
{
    private DekuBehaviour _deku;
    private float _rotationSpeed;
    private float _angle;

    public DekuRotation(DekuBehaviour deku)
    {
        _deku = deku;
        _rotationSpeed = 20f;
    }

    public INode.State Evaluate()
    {
        Transform playerTransform = _deku.PlayerObject.transform;

        Vector3 rotateDirection = (playerTransform.position - _deku.transform.position).normalized;

        _angle = Vector3.Angle(_deku.transform.forward, rotateDirection);

        if(_angle > 10f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotateDirection);

            _deku.transform.rotation = Quaternion.Slerp(_deku.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            return INode.State.Running;
        }

        _deku.transform.rotation = Quaternion.LookRotation(rotateDirection);

        return INode.State.Success;
    }
}
