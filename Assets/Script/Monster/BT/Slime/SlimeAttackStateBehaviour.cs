using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttackStateBehaviour : StateMachineBehaviour
{
    private SlimeBehaviour _slime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_slime == null)
        {
            _slime = animator.GetComponent<SlimeBehaviour>();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _slime.IsAttack = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _slime.IsAttack = false;
    }
}
