using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerRoll : PlayerState, IPlayerState<PlayerRoll>
{
    public PlayerRoll(NewPlayer player) : base(player)
    {
        _rollStateBehaviour = _playerAnimator.GetBehaviour<RollStateBehaviour>();

        player.StartCoroutine(LoadData("P101"));
        _playerInput = player.GetComponent<PlayerInput>();
        _rollAction = _playerInput.actions["Move"];
    }
    
    private RollStateBehaviour _rollStateBehaviour;
    private PlayerInput _playerInput;
    private InputAction _rollAction;

    private readonly int _roll = Animator.StringToHash("Roll");

    public void OnStateEnter()
    {
        _playerAnimator.SetTrigger(_roll);
    }

    public void OnStateFixedUpdate()
    {
        InputCheck();
    }

    public void OnStateExit()
    {
        _input.SetRoll(false);
    }

    private void InputCheck()
    {
        if (!_rollStateBehaviour.IsRoll)
        {
            if(_rollAction.ReadValue<Vector2>() == Vector2.zero)
            {
                
                _state.ChangePlayerState(State.Idle);
            }
            else
            {
                _state.ChangePlayerState(State.Move);
            }

        }
    }

    protected override IEnumerator LoadData(string id)
    {
        yield return new WaitWhile(() => { return DataManager.Instance.GetData(id) == null; });

        _data = DataManager.Instance.GetData(id) as PlayerData;

        if(_rollStateBehaviour != null)
        {
            _rollStateBehaviour.RollSpeed = _data.RollSpeed - 5f;
        }
    }
}
