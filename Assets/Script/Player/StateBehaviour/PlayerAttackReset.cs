using System;
using UnityEngine;

public class PlayerAttackReset : StateMachineBehaviour//부착된 애니메이션 State에서만 활성화.
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

    //애니메이션의 루트모션이 진행되는 동안 매 프레임마다 호출. 애니메이션의 활성화 여부와 관계가 없기 때문에 
    //애니메이션의 루트 모션이 진행되는 중이라면 매 프레임마다 계속 호출된다.
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    //애니매이션이 활성화 되었을 때만 매 프레임마다 호출. 애니메이션이 비활성화 되면 호출되지않음.
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        bool isChargeTrue = stateInfo.shortNameHash == m_Charge_slash_L
            || stateInfo.shortNameHash == m_Charge_slash_R;

        if (isChargeTrue && Input.GetMouseButtonUp(2))
        {
            Debug.Log("실행쓰");
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
