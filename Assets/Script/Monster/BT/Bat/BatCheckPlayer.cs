using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCheckPlayer : INode
{
    private BatBehaviour _bat;
    private float _radius;
    private LayerMask _targetLayer;

    public BatCheckPlayer(BatBehaviour bat)
    {
        _bat = bat;
        _radius = 10f;
        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_bat.CheckPlayer)
        {
            return INode.State.Success;
        }

        Collider[] colliders = Physics.OverlapSphere(_bat.transform.position, _radius, _targetLayer);

        if(colliders.Length > 0)
        {
            _bat.PlayerObject = colliders[0].gameObject;

            _bat.CheckPlayer = true;

            return INode.State.Success;
        }

        return INode.State.Running;
    }
}
