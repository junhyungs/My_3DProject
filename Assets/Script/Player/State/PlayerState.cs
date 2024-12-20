using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public PlayerState(NewPlayer player)
    {
        InitializePlayer(player);
    }

    protected enum InteractionType
    {
        Ladder,
        Item,
        Dialog
    }

    protected NewPlayer _player;
    protected PlayerStateMachine _state;
    protected PlayerInputController _input;
    protected Animator _playerAnimator;
    protected PlayerData _data;
    protected Rigidbody _rigidbody;

    #region Ground
    protected LayerMask _groundMask = LayerMask.GetMask("Ground");
    protected Vector3 _spherePosition;
    protected float _sphereRadius = 0.5f;  
    protected bool _isGround;
    #endregion

    #region Interaction
    private Vector3 _interactionSpherePosition;
    private float _interactionRadius = 0.5f;
    #endregion

    private void InitializePlayer(NewPlayer player)
    {
        _player = player;
        _state = player.GetComponent<PlayerStateMachine>();
        _input = player.GetComponent<PlayerInputController>();
        _playerAnimator = player.GetComponent<Animator>();
        _rigidbody = player.GetComponent<Rigidbody>();
    }

    protected virtual IEnumerator LoadData(string id) {  yield return null; }
    protected void IsGround()
    {
        _spherePosition = _player.transform.position;

        _isGround = Physics.CheckSphere(_spherePosition, _sphereRadius, _groundMask);
        
        if (_isGround)
        {
            return;
        }
        
        _state.ChangePlayerState(State.Falling);
    }

    protected void InteractionLadder()
    {
        _interactionSpherePosition = _player.transform.position + new Vector3(0f, 0.5f, 0f);

        var targetLayer = LayerMask.GetMask("Ladder");

        Collider[] colliders = Physics.OverlapSphere(_interactionSpherePosition, _interactionRadius, targetLayer);

        foreach(var interactionObject in  colliders)
        {
            IInteractionLadder interactionLadder = interactionObject.GetComponent<IInteractionLadder>();

            if (interactionLadder != null)
            {
                interactionLadder.InteractionLadder(_player);

                var ladderState = _state.GetPlayerState(State.Ladder) as PlayerLadder;

                if(ladderState != null)
                {
                    var ladderLength = interactionLadder.LadderLength();

                    ladderState.SetLadderLength(ladderLength);

                    _state.ChangePlayerState(State.Ladder);

                    return;
                }
            }
        }
    }
}
