using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTopBehaviour : StateMachineBehaviour
{
    private PlayerStateMachine _state;
    private Rigidbody _rigidbody;

    private readonly int _climb = Animator.StringToHash("Ladder");
    private float _moveSpeed = 5.0f;

    private Vector3 _moveDirection;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_state == null || _rigidbody == null)
        {
            _state = animator.GetComponent<PlayerStateMachine>();
            _rigidbody = animator.GetComponent<Rigidbody>();
        }

        animator.SetBool(_climb, false);

        _moveDirection = animator.transform.forward;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime <= 0.5f)
        {
            _rigidbody.velocity = _moveDirection * _moveSpeed;
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _state.ChangePlayerState(State.Idle);
    }

}
