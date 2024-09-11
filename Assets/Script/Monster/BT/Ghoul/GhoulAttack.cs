using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulAttack : INode
{
    private GhoulBehaviour _ghoul;

    public GhoulAttack(GhoulBehaviour ghoul)
    {
        _ghoul = ghoul;
    }

    public INode.State Evaluate()
    {
        _ghoul.Animator.SetTrigger("Attack");

        _ghoul.CanMove = false;

        return INode.State.Success;
    }
}
