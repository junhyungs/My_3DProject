using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePatrol : INode
{
    private SlimeBehaviour _slime;
    private Animator _animator;

    public SlimePatrol(SlimeBehaviour slime)
    {
        _slime = slime;
        _animator = _slime.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        if (!_slime.CanMove)
        {
            return INode.State.Running;
        }

        _animator.SetTrigger("Move");

        return INode.State.Success;
    }
}
