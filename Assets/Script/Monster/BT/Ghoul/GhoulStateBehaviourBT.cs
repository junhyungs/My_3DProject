using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GhoulStateBehaviourBT : StateMachineBehaviour
{
    private GhoulBehaviour _ghoul;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_ghoul == null)
        {
            _ghoul = animator.GetComponent<GhoulBehaviour>();   
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _ghoul.CanRotation = false;        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        _ghoul.CanRotation = true;
        _ghoul.CanMove = true;
    }
}