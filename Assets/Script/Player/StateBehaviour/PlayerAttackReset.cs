using System;
using UnityEngine;

public class PlayerAttackReset : StateMachineBehaviour//������ �ִϸ��̼� State������ Ȱ��ȭ.
{
    [Header("TriggerName")]
    [SerializeField] private string m_triggerName;
    [SerializeField] private string m_ChargeL;
    [SerializeField] private string m_ChargeR;
    [SerializeField] private string m_ChargeFail;

    private PlayerWeaponController m_objectController;
    private PlayerMoveController m_moveController;
    private PlayerAttackController m_attackController;

    #region Animator.StringToHash
    private int m_Slash_Light_L = Animator.StringToHash("Slash_Light_L");
    private int m_Slash_Light_R = Animator.StringToHash("Slash_Light_R");
    private int m_Slash_Light_Last = Animator.StringToHash("Slash_Light_L_Last");
    private int m_Charge_slash_L = Animator.StringToHash("Charge_slash_L");
    private int m_Charge_slash_R = Animator.StringToHash("Charge_slash_R");
    private int m_Charge_MaxL = Animator.StringToHash("Charge_max_L");
    private int m_Charge_MaxR = Animator.StringToHash("Charge_max_R");
    #endregion

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChargeMax(stateInfo, false);
        animator.ResetTrigger(m_triggerName);
        animator.ResetTrigger(m_ChargeL);
        animator.ResetTrigger(m_ChargeR);
        animator.ResetTrigger(m_ChargeFail);
        animator.SetBool("NextAttack", false);
        m_moveController.IsAction = true;
    }

    //�ִϸ��̼��� ��Ʈ����� ����Ǵ� ���� �� �����Ӹ��� ȣ��. �ִϸ��̼��� Ȱ��ȭ ���ο� ���谡 ���� ������ 
    //�ִϸ��̼��� ��Ʈ ����� ����Ǵ� ���̶�� �� �����Ӹ��� ��� ȣ��ȴ�.
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    //�ִϸ��̼��� Ȱ��ȭ �Ǿ��� ���� �� �����Ӹ��� ȣ��. �ִϸ��̼��� ��Ȱ��ȭ �Ǹ� ȣ���������.
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        bool isChargeTrue = stateInfo.shortNameHash == m_Charge_slash_L
            || stateInfo.shortNameHash == m_Charge_slash_R;

        if (isChargeTrue && Input.GetMouseButtonUp(2))
        {
            Debug.Log("���ྲ");
            animator.SetTrigger("ChargeFail");
        }

        m_moveController.IsAction = false;
    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetComponent(animator);
        ChargeMax(stateInfo, true);
        //AnimationStateMove(stateInfo, m_attackController.IsChargeMax);
        ActiveObject(stateInfo);
    }

    private void GetComponent(Animator animator)
    {
        bool isNull = m_attackController == null
            || m_moveController == null
            || m_objectController == null;

        if (isNull)
        {
            m_moveController = animator.GetComponent<PlayerMoveController>();
            m_attackController = animator.GetComponent<PlayerAttackController>();
            m_objectController = animator.GetComponent<PlayerWeaponController>();
        }
    }

    private void ChargeMax(AnimatorStateInfo stateInfo, bool Value)
    {
        bool IsChargeMax = stateInfo.shortNameHash == m_Charge_MaxL
            || stateInfo.shortNameHash == m_Charge_MaxR;

        if (IsChargeMax)
        {
            m_attackController.IsChargeMax = Value;
        }
    }

    //private void AnimationStateMove(AnimatorStateInfo stateInfo, bool isChargeMax)
    //{
    //    bool animationStateMove = stateInfo.shortNameHash == m_Slash_Light_L
    //       || stateInfo.shortNameHash == m_Slash_Light_R
    //       || stateInfo.shortNameHash == m_Slash_Light_Last;

    //    if (animationStateMove)
    //    {
    //        m_moveController.AnimationStateMove(isChargeMax);
    //    }
    //}

    private void ActiveObject(AnimatorStateInfo stateInfo)
    {
        var actionDic = m_objectController.ActiveWeaponDic;

        if (actionDic.TryGetValue(stateInfo.shortNameHash, out Action<bool> action))
        {
            action(m_attackController.IsChargeMax);
        }
    }
}
