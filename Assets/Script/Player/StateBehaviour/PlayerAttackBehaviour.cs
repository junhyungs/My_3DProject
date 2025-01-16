using UnityEngine;
using System;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    #region Component
    private PlayerWeaponController _weaponController;
    private PlayerMoveController _moveController;
    private PlayerAttackController _attackController;
    #endregion

    #region Animator_StringToHash
    private readonly int _attackTrigger = Animator.StringToHash("Attack");
    private readonly int _charge_Left = Animator.StringToHash("ChargeAttackL");
    private readonly int _charge_Right = Animator.StringToHash("ChargeAttackR");
    private readonly int _charge_Fail = Animator.StringToHash("ChargeFail");
    private readonly int _charge_Max_Left = Animator.StringToHash("Charge_max_L");
    private readonly int _charge_Max_Right = Animator.StringToHash("Charge_max_R");
    private readonly int _charge_Slash_Left = Animator.StringToHash("Charge_slash_L");
    private readonly int _charge_Slash_Right = Animator.StringToHash("Charge_slash_R");
    private readonly int _nextAttack = Animator.StringToHash("NextAttack");
    #endregion

    private int[] _triggerArray;

    private void Awake()
    {
        _triggerArray = new int[]
        {
            _attackTrigger,
            _charge_Left,
            _charge_Right,
            _charge_Fail
        };
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetController(animator);
        ChargeMax(stateInfo, true);
        ActiveWeaponObject(stateInfo);
    }

    private void GetController(Animator animator)
    {
        _weaponController = GetComponent(_weaponController, animator);
        _moveController = GetComponent(_moveController, animator);
        _attackController = GetComponent(_attackController, animator);
    }

    private T GetComponent<T>(T component, Animator animator) where T : Component
    {
        return component ?? animator.GetComponent<T>();
    }

    private void ChargeMax(AnimatorStateInfo stateInfo, bool value)
    {
        if(IsChargeMaxAnimation(stateInfo))
        {
            _attackController.IsChargeMax = value;
        }
    }

    private void ActiveWeaponObject(AnimatorStateInfo stateInfo)
    {
        var actionDictionary = _weaponController.ActiveWeaponDic;

        if(actionDictionary.TryGetValue(stateInfo.shortNameHash, out Action<bool> action))
        {
            action(_attackController.IsChargeMax);
        }
    }

    private bool IsChargeMaxAnimation(AnimatorStateInfo stateInfo)
    {
        return stateInfo.shortNameHash == _charge_Max_Left || stateInfo.shortNameHash == _charge_Max_Right;
    }

    private bool IsChargeSlashAnimation(AnimatorStateInfo stateInfo)
    {
        return stateInfo.shortNameHash == _charge_Slash_Left || stateInfo.shortNameHash == _charge_Slash_Right;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _moveController.IsAction = false;

        if(IsChargeSlashAnimation(stateInfo) && Input.GetMouseButtonUp(2))
        {
            animator.SetTrigger(_charge_Fail);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChargeMax(stateInfo, false);
        ResetTrigger(animator);

        animator.SetBool(_nextAttack, false);
        _moveController.IsAction = true;
    }

    private void ResetTrigger(Animator animator)
    {
        for(int i = 0; i < _triggerArray.Length; i++)
        {
            animator.ResetTrigger(_triggerArray[i]);
        }
    }
}
