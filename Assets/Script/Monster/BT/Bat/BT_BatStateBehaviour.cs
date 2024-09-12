using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_BatStateBehaviour : StateMachineBehaviour
{
    private BatBehaviour batBehaviour;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(batBehaviour == null)
        {
            batBehaviour = animator.GetComponent<BatBehaviour>();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        batBehaviour.IsAttack = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        batBehaviour.IsAttack = false;
    }
}
