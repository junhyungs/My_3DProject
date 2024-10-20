using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCheckPlayer : INode
{
    private MageBehaviour _mage;
    private float _radius;
    private LayerMask _targetLayer;

    public MageCheckPlayer(MageBehaviour mageBehaviour)
    {
        _mage = mageBehaviour;

        _radius = 7f;
        _targetLayer = LayerMask.GetMask("Player");
    }

    public INode.State Evaluate()
    {
        if (_mage.CheckPlayer)
        {
            return INode.State.Success;
        }

        Collider[] colliders = Physics.OverlapSphere(_mage.transform.position, _radius, _targetLayer);

        if(colliders.Length > 0)
        {
            _mage.PlayerObject = colliders[0].gameObject;

            _mage.CheckPlayer = true;

            return INode.State.Fail;
        }

        return INode.State.Running;
    }
}
