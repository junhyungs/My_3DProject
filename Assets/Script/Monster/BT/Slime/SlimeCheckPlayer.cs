using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCheckPlayer : INode
{
    private SlimeBehaviour _slime;
    private float _radius;
    private LayerMask _targetLayer;

    public SlimeCheckPlayer(SlimeBehaviour slime)
    {
        _slime = slime;

        _radius = 5f;
        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_slime.CheckPlayer)
        {
            return INode.State.Success;
        }

        Collider[] colliders = Physics.OverlapSphere(_slime.transform.position, _radius, _targetLayer);

        if(colliders.Length > 0)
        {
            _slime.PlayerObject = colliders[0].gameObject;

            _slime.CheckPlayer = true;

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
