using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState, IPlayerState<PlayerIdle>
{
    public PlayerIdle(NewPlayer player) : base(player) { }

    private readonly int _idle = Animator.StringToHash("MoveSpeed");
    
    public void OnStateEnter()
    {
        var blendTreeValue = _playerAnimator.GetFloat(_idle);

        if(blendTreeValue != 0)
        {
            _playerAnimator.SetFloat(_idle, 0f);
        }
    }

    public void OnStateFixedUpdate()
    {
        IsGround();
        InputCheck();
    }

    public void OnStateExit()
    {

    }

    private void InputCheck()
    {
        if(_input.InputValue != Vector2.zero)
        {
            _state.ChangePlayerState(State.Move);
        }
        else if (_input.IsRoll)
        {
            _state.ChangePlayerState(State.Roll);
        }
        else if (_input.IsLadder)
        {
            InteractionLadder();
        }
    }

}
