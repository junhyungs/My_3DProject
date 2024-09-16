using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuCheckPlayer : INode
{
    private DekuBehaviour _deku;
    private Animator _animator;
    private float _radius;

    public DekuCheckPlayer(DekuBehaviour deku)
    {
        _deku = deku;
        _animator = _deku.GetComponent<Animator>(); 

        _radius = 10f;
    }

    public INode.State Evaluate()
    {
        if (_deku.CheckPlayer)
        {
            return INode.State.Success;
        }

        if (_deku.IsReturn)
        {
            return INode.State.Fail;
        }

        Collider[] colliders = Physics.OverlapSphere(_deku.transform.position, _radius, LayerMask.GetMask("Player"));

        if(colliders.Length > 0 )
        {
            _animator.SetBool("Hide", false);

            _deku.PlayerObject = colliders[0].gameObject;

            _deku.CheckPlayer = true;

            return INode.State.Success;
        }

        return INode.State.Fail;
    }
}
