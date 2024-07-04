using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuStateBehaviour : StateMachineBehaviour
{
    DEKU_SCRUB m_monster;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        m_monster.IsAction = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_monster == null)
        {
            m_monster = animator.GetComponent<DEKU_SCRUB>();
        }

        m_monster.IsAction = true;

    }

}
