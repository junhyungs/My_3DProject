using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAttackReset : StateMachineBehaviour
{
    [Header("TriggerName")]
    [SerializeField] private string m_triggerName;

    private PlayerAttackController m_controller;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(m_triggerName);
        animator.SetBool("NextAttack", false);


        //if (m_controller == null)
        //    m_controller = animator.GetComponent<PlayerAttackController>();

        //if (stateInfo.shortNameHash == Animator.StringToHash("Slash_Light_L"))
        //{
        //    m_controller.ActiveLeftWeaponObject(WeaponManager.Instance.GetCurrentWeapon(), false);
        //}
        //else if(stateInfo.shortNameHash == Animator.StringToHash("Slash_Light_R"))
        //{
        //    m_controller.ActiveRightWeaponObject(WeaponManager.Instance.GetCurrentWeapon(), false);
        //}
        //else if(stateInfo.shortNameHash == Animator.StringToHash("Slash_Light_L_Last"))
        //{
        //    m_controller.ActiveLeftWeaponObject(WeaponManager.Instance.GetCurrentWeapon(), false);
        //}

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (m_controller == null)
        //    m_controller = animator.GetComponent<PlayerAttackController>();

        //if (stateInfo.shortNameHash == Animator.StringToHash("Slash_Light_L"))
        //{
        //    m_controller.ActiveLeftWeaponObject(WeaponManager.Instance.GetCurrentWeapon(), true);
        //}
        //else if (stateInfo.shortNameHash == Animator.StringToHash("Slash_Light_R"))
        //{
        //    m_controller.ActiveRightWeaponObject(WeaponManager.Instance.GetCurrentWeapon(), true);
        //}
        //else if (stateInfo.shortNameHash == Animator.StringToHash("Slash_Light_L_Last"))
        //{
        //    m_controller.ActiveLeftWeaponObject(WeaponManager.Instance.GetCurrentWeapon(), true);
        //}
    }


}
