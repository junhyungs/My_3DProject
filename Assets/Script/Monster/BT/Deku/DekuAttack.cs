using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuAttack : INode
{
    private DekuBehaviour _deku;
    private Animator _animator;

    public DekuAttack(DekuBehaviour deku)
    {
        _deku = deku;
        _animator = _deku.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        _deku.IsAttack = true;

        _animator.SetTrigger("Attack");

        return INode.State.Success;
    }
}
