using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAttackReset : StateMachineBehaviour//부착된 애니메이션 State에서만 활성화.
{
    [Header("TriggerName")]
    [SerializeField] private string m_triggerName;

    private PlayerWeaponController m_objectController;
    private PlayerMoveController m_moveController;
    private int m_Slash_Light_L = Animator.StringToHash("Slash_Light_L");
    private int m_Slash_Light_R = Animator.StringToHash("Slash_Light_R");
    private int m_Slash_Light_Last = Animator.StringToHash("Slash_Light_L_Last");

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(m_triggerName);
        animator.SetBool("NextAttack", false);

        if(m_moveController == null)
        {
            m_moveController = animator.GetComponent<PlayerMoveController>();
        }

        m_moveController.IsAction = true;

    }

    //애니메이션의 루트모션이 진행되는 동안 매 프레임마다 호출. 애니메이션의 활성화 여부와 관계가 없기 때문에 
    //애니메이션의 루트 모션이 진행되는 중이라면 매 프레임마다 계속 호출된다.
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    
    //애니매이션이 활성화 되었을 때만 매 프레임마다 호출. 애니메이션이 비활성화 되면 호출되지않음.
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_moveController == null)
        {
            m_moveController = animator.GetComponent<PlayerMoveController>();
        }

        m_moveController.IsAction = false;
    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_objectController == null || m_moveController == null)
        {
            m_objectController = animator.GetComponent<PlayerWeaponController>();
            m_moveController = animator.GetComponent<PlayerMoveController>();
        }   


        m_moveController.AnimationStateMove();

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
