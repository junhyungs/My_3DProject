using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss
{
    Idle,
    Move,
    Trace,
    Attack
}

public class BossStateMachine : MonoBehaviour
{
    private Dictionary<Boss, Boss_BaseState> StateDic = new Dictionary<Boss, Boss_BaseState>();
    private Boss_BaseState m_currentState;

    private void Start()
    {
        m_currentState = StateDic[Boss.Idle];
        m_currentState.StateEnter();
    }

    private void Update()
    {
        m_currentState.StateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_currentState.StateTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        m_currentState.StateTriggerExit(other);
    }

    public void AddState(Boss state, Boss_BaseState newStateClass)
    {
        StateDic.Add(state, newStateClass);
    }

    public void ChangeState(Boss newState)
    {
        m_currentState.StateExit();
        m_currentState = StateDic[newState];
        m_currentState.StateEnter();
    }
}
