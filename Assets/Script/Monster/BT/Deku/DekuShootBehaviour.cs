using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuShootBehaviour : StateMachineBehaviour
{
    private DekuBehaviour _deku;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_deku == null)
        {
            _deku = animator.GetComponent<DekuBehaviour>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        _deku.IsAttack = false;
    }
}
