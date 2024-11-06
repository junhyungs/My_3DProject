using UnityEngine;

public class PlayerRollStateBehaviour : StateMachineBehaviour
{
    private PlayerMoveController _moveController;
    private CharacterController _playerCharaterController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);

        _moveController.IsAction = false;
        animator.SetBool("isRoll", false);
    }

    private void Initialize(Animator animator)
    {
        if (_moveController != null)
            return;

        _moveController = animator.GetComponent<PlayerMoveController>();
        _playerCharaterController = animator.GetComponent<CharacterController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime <= 0.85)
        {
            Vector3 rollDirection = (_moveController.transform.forward).normalized * _moveController.m_rollSpeed * Time.deltaTime;

            _playerCharaterController.Move(rollDirection);
        }
        else
        {
            _moveController.IsAction = true;
        }
    }
}
