using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : MonoBehaviour
{
    private void Awake()
    {
        AddComponent();
    }

    private void AddComponent()
    {
        gameObject.AddComponent<PlayerInputController>();
        gameObject.AddComponent<PlayerCamera>();
        AddState();
    }

    private void AddState()
    {
        var stateMachine = gameObject.AddComponent<PlayerStateMachine>();

        stateMachine.AddPlayerState(State.Idle, new PlayerIdle(this));
        stateMachine.AddPlayerState(State.Move, new PlayerMove(this));
        stateMachine.AddPlayerState(State.Falling, new PlayerFalling(this));
        stateMachine.AddPlayerState(State.Roll, new PlayerRoll(this));
        stateMachine.AddPlayerState(State.Ladder, new PlayerLadder(this));  
    }
}
