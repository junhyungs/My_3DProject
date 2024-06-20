using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillBehaviour : StateMachineBehaviour
{
    private PlayerMoveController m_moveController;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_moveController == null)
        {
            m_moveController = animator.GetComponent<PlayerMoveController>();
        }

        m_moveController.IsAction = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_moveController.IsAction = true;
    }
}
