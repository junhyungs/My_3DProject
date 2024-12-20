using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : PlayerState, IPlayerState<PlayerFalling>
{
    public PlayerFalling(NewPlayer player) : base(player) { }

    private readonly int _falling = Animator.StringToHash("Falling");

    public void OnStateEnter()
    {
        _playerAnimator.SetBool(_falling, true);
    }

    public void OnStateFixedUpdate()
    {
        _spherePosition = _player.transform.position;

        _isGround = Physics.CheckSphere(_spherePosition, _sphereRadius, _groundMask);

        if (_isGround)
        {
            _state.ChangePlayerState(State.Idle);
        }
    }
    public void OnStateExit()
    {
        _playerAnimator.SetBool(_falling, false);
    }
}
