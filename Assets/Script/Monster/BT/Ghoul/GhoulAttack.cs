using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulAttack : INode
{
    private GhoulBehaviour _ghoul;
    private Animator _animator;

    public GhoulAttack(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;
        _animator = _ghoul.GetComponent<Animator>();
    }

    public INode.State Evaluate()
    {
        _ghoul.IsAttack = true;

        _animator.SetTrigger("Attack");

        return INode.State.Success;
    }
}
