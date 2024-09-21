using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuHide : INode
{
    private DekuBehaviour _deku;
    private Animator _animator;

    public DekuHide(DekuBehaviour deku)
    {
        _deku = deku;
        _animator = _deku.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        _animator.SetBool("Hide", true);

        return INode.State.Success;
    }
}
