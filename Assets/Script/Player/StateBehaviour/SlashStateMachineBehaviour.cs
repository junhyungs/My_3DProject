using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashStateMachineBehaviour : StateMachineBehaviour
{
    private PlayerAttackController _attackController;
    private CharacterController _characterController;

    private Vector3 _moveDirection;

    private float _moveSpeed;
    private float _chargeSpeed = 10f;
    private float _basicSpeed = 3f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attackController == null
            || _characterController == null)
        {
            _attackController = animator.GetComponent<PlayerAttackController>();
            _characterController = animator.GetComponent<CharacterController>();
        }

        _moveDirection = _attackController.transform.forward;

        _moveSpeed = _attackController.IsChargeMax ? _chargeSpeed : _basicSpeed;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime < 0.2f)
        {
            _characterController.Move(_moveDirection * _moveSpeed * Time.deltaTime);
        }
    }
}
