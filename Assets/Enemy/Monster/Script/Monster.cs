using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterData
{
    Bat = 1
}

public abstract class Monster : MonoBehaviour, IDamged
{
    protected MonsterStateMachine m_monsterStateMachine;
    protected NavMeshAgent m_monsterAgent;
    protected Animator m_monsterAnim;
    protected GameObject m_player;
    protected int m_monsterHealth;
    protected int m_monsterAttackPower;
    protected float m_monsterSpeed;

    public abstract void TakeDamage(int damage);
    
    protected virtual void Start()
    {
        m_monsterAgent = GetComponent<NavMeshAgent>();
        m_monsterAnim = GetComponent<Animator>();
        m_player = GameManager.Instance.Player;
    }









}



