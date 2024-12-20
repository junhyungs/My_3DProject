using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Dictionary<State, IPlayerState> _stateDictionary = new Dictionary<State, IPlayerState>();
    private IPlayerState _currentState;

    private void Start()
    {
        _currentState = _stateDictionary[State.Idle];

        _currentState.OnStateEnter();
    }

    private void FixedUpdate()
    {
        _currentState.OnStateFixedUpdate();
    }

    public void ChangePlayerState(State stateType)
    {
        _currentState.OnStateExit();

        if(_stateDictionary.TryGetValue(stateType, out IPlayerState iplayerState))
        {
            _currentState = iplayerState;

            _currentState.OnStateEnter();
        }
    }

    public void AddPlayerState(State stateType, IPlayerState newState)
    {
        if (_stateDictionary.ContainsKey(stateType))
        {
            return;
        }

        _stateDictionary.Add(stateType, newState);
    }

    public IPlayerState GetPlayerState(State state)
    {
        if(_stateDictionary.TryGetValue(state, out IPlayerState playerState))
        {
            return playerState;
        }

        return null;    
    }
}

public interface IPlayerState
{
    public void OnStateEnter();
    public void OnStateFixedUpdate();
    public void OnStateExit();
}

public interface IPlayerState<T> : IPlayerState where T : IPlayerState<T> { }