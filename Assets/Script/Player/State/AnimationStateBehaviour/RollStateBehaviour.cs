using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollStateBehaviour : StateMachineBehaviour
{
    private Rigidbody _rigidbody;

    private Vector3 _rollDirection;
    private Vector3 _rollVector;
    private Vector3 _exitVector;

    private bool _isRoll;
    public bool IsRoll => _isRoll;
    public float RollSpeed { get; set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_rigidbody == null)
        {
            _rigidbody = animator.GetComponent<Rigidbody>();

            _exitVector = new Vector3(0f, _rigidbody.velocity.y, 0f);
        }

        RollState(true);

        Roll(animator);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        RollState(false);
    }

    private void RollState(bool isRoll)
    {
        _isRoll = isRoll;

        _rigidbody.velocity = _exitVector;
    }

    private void Roll(Animator animator)
    {
        _rollDirection = (animator.transform.forward).normalized;

        _rollVector = _rollDirection * RollSpeed;

        _rigidbody.AddForce(_rollVector,ForceMode.Impulse);
    }
}
