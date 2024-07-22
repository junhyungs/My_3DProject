using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Dash,
    Jump,
    MegaDash,
    Scream,
    Egg
}

public class BossStateMachine : MonoBehaviour
{
    private Dictionary<BossState, Boss_BaseState> StateDic = new Dictionary<BossState, Boss_BaseState>();
    private Boss_BaseState m_currentState;

    private void Start()
    {
        m_currentState = StateDic[BossState.Dash];
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

    public void AddState(BossState state, Boss_BaseState newStateClass)
    {
        StateDic.Add(state, newStateClass);
    }

    public void ChangeState(BossState newState)
    {
        m_currentState.StateExit();
        m_currentState = StateDic[newState];
        m_currentState.StateEnter();
    }
}
