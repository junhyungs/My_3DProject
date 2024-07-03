using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillBehaviour : StateMachineBehaviour
{
    private PlayerMoveController m_moveController;
    private int m_Hookshot_fly = Animator.StringToHash("Hookshot_fly");

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

        if(stateInfo.shortNameHash == m_Hookshot_fly)
        {
            animator.SetBool("Hook", false);
        }
    }
}
