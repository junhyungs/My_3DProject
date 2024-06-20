using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAttackReset : StateMachineBehaviour
{
    [Header("TriggerName")]
    [SerializeField] private string m_triggerName;

    private PlayerWeaponController m_objectController;
    private PlayerMoveController m_moveController;
    private int m_Slash_Light_L = Animator.StringToHash("Slash_Light_L");
    private int m_Slash_Light_R = Animator.StringToHash("Slash_Light_R");
    private int m_Slash_Light_Last = Animator.StringToHash("Slash_Light_L_Last");
    private int m_Idle = Animator.StringToHash("");

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(m_triggerName);
        animator.SetBool("NextAttack", false);

        if(m_moveController == null)
        {
            m_moveController = animator.GetComponent<PlayerMoveController>();
        }

        bool playAttackAnimation = stateInfo.shortNameHash == m_Slash_Light_L
            || stateInfo.shortNameHash == m_Slash_Light_R
            || stateInfo.shortNameHash == m_Slash_Light_Last;

        if (playAttackAnimation)
        {
            Debug.Log("OnStateExitCall");
            m_moveController.IsAction = true;
        }

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_moveController == null)
        {
            m_moveController = animator.GetComponent<PlayerMoveController>();
        }

        bool playAttackAnimation = stateInfo.shortNameHash == m_Slash_Light_L 
            || stateInfo.shortNameHash == m_Slash_Light_R
            || stateInfo.shortNameHash == m_Slash_Light_Last;

        if (playAttackAnimation && m_moveController.IsAction)
        {
            m_moveController.IsAction = false;
        }
        

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_objectController == null)
        {
            m_objectController = animator.GetComponent<PlayerWeaponController>();
        }   

        if(stateInfo.shortNameHash == m_Slash_Light_L)
        {
            m_objectController.ActiveRightWeaponObject();
        }
        else if(stateInfo.shortNameHash == m_Slash_Light_R)
        {
            m_objectController.ActiveLeftWeaponObject();
        }
        else if(stateInfo.shortNameHash == m_Slash_Light_Last)
        {
            m_objectController.ActiveRightWeaponObject();
        }
    }
}
