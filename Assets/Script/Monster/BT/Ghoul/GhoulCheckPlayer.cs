using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulCheckPlayer : INode
{
    private GhoulBehaviour _ghoul;
    private float _radius;
    private LayerMask _targetLayer;

    public GhoulCheckPlayer(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;

        _radius = 10f;
        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_ghoul.CheckPlayer)
        {
            return INode.State.Success;
        }

        Collider[] colliders = Physics.OverlapSphere(_ghoul.transform.position, _radius, _targetLayer);

        if(colliders.Length > 0)
        {
            _ghoul.PlayerObject = colliders[0].gameObject;

            _ghoul.CheckPlayer = true;

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
