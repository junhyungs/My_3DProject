using UnityEngine;

public class PlayerRollStateBehaviour : StateMachineBehaviour
{
    #region Component
    private PlayerMoveController _moveController;
    private CharacterController _playerCharaterController;
    #endregion

    private readonly int _roll = Animator.StringToHash("isRoll");

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetController(animator);

        _moveController.IsAction = false;
        animator.SetBool(_roll, false);
    }

    private void GetController(Animator animator)
    {
        _moveController = GetComponent(_moveController, animator);
        _playerCharaterController = GetComponent(_playerCharaterController, animator);  
    }

    private T GetComponent<T>(T compoent, Animator animator) where T : Component
    {
        return compoent ?? animator.GetComponent<T>();
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
