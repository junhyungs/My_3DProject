using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbStateBehaviour : StateMachineBehaviour
{
    PlayerMoveController m_moveContorller;
    PlayerAttackController m_attackController;

    #region Animator.StringToHash
    private int m_Climbing_off_ladder_top = Animator.StringToHash("Climbing_off_ladder_top");
    private int m_Climbing_ladder_down = Animator.StringToHash("Climbing_ladder_down");
    private int m_Climbing_Idle = Animator.StringToHash("Climbing_Idle");
    private int m_Climbing_ladder = Animator.StringToHash("Climbing_ladder");
    #endregion

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetComponent(animator);

        if (IsClimbExit(stateInfo))
        {
            m_attackController.IsAction = true;
            m_moveContorller.IsAction = true;
            m_moveContorller.IsLadder = false;
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetComponent(animator);

        m_attackController.IsAction = false;
        m_moveContorller.IsAction = false;
        m_moveContorller.IsLadder = true;

        if (IsClimbExit(stateInfo))
        {
            m_moveContorller.ClimbStateMove();
        }
    }

    private void GetComponent(Animator animator)
    {
        if (m_moveContorller == null || m_attackController == null)
        {
            m_moveContorller = animator.GetComponent<PlayerMoveController>();
            m_attackController = animator.GetComponent<PlayerAttackController>();
        }
    }

    private bool IsClimbExit(AnimatorStateInfo stateInfo)
    {
        return stateInfo.shortNameHash == m_Climbing_off_ladder_top;
    }

    
}
