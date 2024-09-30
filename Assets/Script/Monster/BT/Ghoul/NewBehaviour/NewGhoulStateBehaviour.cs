using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGhoulStateBehaviour : StateMachineBehaviour
{
    private Ghoul_BT _ghoul;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_ghoul == null)
        {
            _ghoul = animator.GetComponent<Ghoul_BT>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _ghoul.IsAttack = false;
    }
}
