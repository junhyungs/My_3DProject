using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulStateBehaviour : StateMachineBehaviour
{
    private Ghoul m_ghoul;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_ghoul == null)
        {
            m_ghoul = animator.GetComponent<Ghoul>();
        }

        m_ghoul.IsAction = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_ghoul.IsAction = false;
    }
}
