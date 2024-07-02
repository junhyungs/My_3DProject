using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle,
    Move,
    Trace,
    Attack,
    TelePort,
    Hide,
    SelfDestruction,
}
public class MonsterStateMachine : MonoBehaviour
{
    private Dictionary<MonsterState, Monster_BaseState> StateDic = new Dictionary<MonsterState, Monster_BaseState>();
    private Monster_BaseState m_currentState;

    private void Start()
    {
        m_currentState = StateDic[MonsterState.Idle];
        m_currentState.StateEnter();
    }

    private void Update()
    {
        m_currentState.StateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_currentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        m_currentState.StateOnTriggerExit(other);
    }

    public void AddState(MonsterState state, Monster_BaseState newStateClass)
    {
        StateDic.Add(state, newStateClass);
    }

    public void ChangeState(MonsterState newState)
    {
        m_currentState.StateExit();
        m_currentState = StateDic[newState];
        m_currentState.StateEnter();
    }
}
