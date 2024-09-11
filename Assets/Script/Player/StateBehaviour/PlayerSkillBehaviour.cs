using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillBehaviour : StateMachineBehaviour
{
    [Header("ResetTrigger")]
    [SerializeField] private string m_Hook;

    private PlayerMoveController m_moveController;
    private PlayerAttackController m_attackController;
    private int m_Hookshot_fly = Animator.StringToHash("Hookshot_fly");
    private int m_Arrow = Animator.StringToHash("Arrow");

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_moveController == null || m_attackController == null)
        {
            m_moveController = animator.GetComponent<PlayerMoveController>();
            m_attackController = animator.GetComponent<PlayerAttackController>();
        }

        if(stateInfo.shortNameHash == m_Hookshot_fly)
        {
            m_attackController.IsAction = false;
        }        
        m_moveController.IsAction = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(m_Hook);
        m_moveController.IsAction = true;
        m_attackController.IsAction = true;
    }
}
