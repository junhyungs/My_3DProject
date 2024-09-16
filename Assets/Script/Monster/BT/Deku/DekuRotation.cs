using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DekuRotation : INode
{
    private DekuBehaviour _deku;
    private NavMeshAgent _agent;
    private float _rotationSpeed;

    public DekuRotation(DekuBehaviour deku)
    {
        _deku = deku;
        _agent = _deku.GetComponent<NavMeshAgent>();

        _rotationSpeed = 20f;
    }

    public INode.State Evaluate()
    {
        Transform playerTransform = _deku.PlayerObject.transform;

        Vector3 rotateDirection = (playerTransform.position - _deku.transform.position).normalized;

        rotateDirection.y = 0f;

        float angle = Vector3.Angle(_deku.transform.forward, rotateDirection);

        if(angle > 1.5f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotateDirection);

            _deku.transform.rotation = Quaternion.Slerp(_deku.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            return INode.State.Running;
        }

        return INode.State.Success;
    }
}
