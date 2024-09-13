using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCanRotation : INode
{
    private SlimeBehaviour _slime;

    public SlimeCanRotation(SlimeBehaviour slime)
    {
        _slime = slime;
    }

    public INode.State Evaluate()
    {
        if (!_slime.CheckPlayer)
        {
            return INode.State.Fail;
        }

        Transform playerTransform = _slime.PlayerObject.transform;

        Vector3 rotateDirection = (playerTransform.position - _slime.transform.position).normalized;

        float angle = Vector3.Angle(_slime.transform.forward, rotateDirection);

        if(angle > 1f)
        {
            rotateDirection.y = 0f;

            Quaternion rotation = Quaternion.LookRotation(rotateDirection);

            _slime.transform.rotation = Quaternion.Slerp(_slime.transform.rotation, rotation, 10f * Time.deltaTime);

            return INode.State.Success;
        }

        return INode.State.Success;
    }
}
