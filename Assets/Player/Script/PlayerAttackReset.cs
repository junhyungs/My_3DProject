using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAttackReset : StateMachineBehaviour//������ �ִϸ��̼� State������ Ȱ��ȭ.
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

    //�ִϸ��̼��� ��Ʈ����� ����Ǵ� ���� �� �����Ӹ��� ȣ��. �ִϸ��̼��� Ȱ��ȭ ���ο� ���谡 ���� ������ 
    //�ִϸ��̼��� ��Ʈ ����� ����Ǵ� ���̶�� �� �����Ӹ��� ��� ȣ��ȴ�.
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    
    //�ִϸ��̼��� Ȱ��ȭ �Ǿ��� ���� �� �����Ӹ��� ȣ��. �ִϸ��̼��� ��Ȱ��ȭ �Ǹ� ȣ���������.
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
