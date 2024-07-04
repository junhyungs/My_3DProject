using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Ghoul : Monster
{
    protected override void Start()
    {
        base.Start();
        InitStateMachine();
        InitGhoul();
        InitMaterial();
    }

    private void InitStateMachine()
    {
        m_monsterStateMachine = gameObject.AddComponent<MonsterStateMachine>();
        m_monsterStateMachine.AddState(MonsterState.Idle, new GhoulIdleState(this));
        m_monsterStateMachine.AddState(MonsterState.Move, new GhoulMoveState(this));
        m_monsterStateMachine.AddState(MonsterState.Trace, new GhoulTraceState(this));
        m_monsterStateMachine.AddState(MonsterState.Attack, new GhoulAttackState(this));
    }

    private void InitGhoul()
    {
        int id = (int)MonsterData.Ghoul;
        m_monsterHealth = DataManager.Instance.GetMonsterData(id).MonsterHp;
        m_monsterAttackPower = DataManager.Instance.GetMonsterData(id).MonsterAttackPower;
        m_monsterSpeed = DataManager.Instance.GetMonsterData(id).MonsterSpeed;
        m_monsterAgent.speed = m_monsterSpeed;
    }

    private void InitMaterial()
    {
        m_copyMaterial = Instantiate(m_originalMaterial);
        m_skinnedMeshRenderer.material = m_copyMaterial;
        m_saveColor = m_copyMaterial.GetColor("_Color");
    }

    public override void TakeDamage(float damage)
    {
        m_monsterHealth -= (int)damage;

        if (m_monsterHealth <= 0)
        {
            Die();
        }
        else
            StartCoroutine(IntensityChange(2f, 3f));
    }

    private void Die()
    {
        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        m_monsterAgent.isStopped = true;
        m_monsterRigid.isKinematic = true;
        m_monsterAnim.SetTrigger("Die");
        StartCoroutine(Die(5f, 0.5f, 0.001f));
    }

    public MonsterStateMachine State
    {
        get { return m_monsterStateMachine; }
    }

    public Animator Anim
    {
        get { return m_monsterAnim; }
    }

    public GameObject Player
    {
        get { return m_player; }
    }

    public NavMeshAgent Agent
    {
        get { return m_monsterAgent; }
    }
}

public abstract class GhoulState : Monster_BaseState
{
    protected Ghoul m_Ghoul;
    public GhoulState(Ghoul ghoul)
    {
        m_Ghoul = ghoul;
    }   
}

public class GhoulIdleState : GhoulState
{
    public GhoulIdleState(Ghoul ghoul) : base(ghoul)
    {
    }
    private float m_IdleTime;

    public override void StateEnter()
    {
        m_Ghoul.StartCoroutine(ChangeTimer());
    }

    public override void StateExit()
    {
        m_IdleTime = 0f;
    }

    private IEnumerator ChangeTimer()
    {
        while (true)
        {
            m_IdleTime += Time.deltaTime;

            yield return null;

            if(m_IdleTime >= 1f)
            {
                m_Ghoul.State.ChangeState(MonsterState.Move);
                yield break;
            }
        }
    }

}

public class GhoulMoveState : GhoulState
{
    public GhoulMoveState(Ghoul ghoul) : base(ghoul)
    {
    }

    private List<Transform> WayPointList = new List<Transform>();

    public override void StateEnter()
    {
        FindWayPoints();
    }

    public override void StateUpdate()
    {
        if (m_Ghoul.Agent.remainingDistance <= m_Ghoul.Agent.stoppingDistance)
        {
            m_Ghoul.State.ChangeState(MonsterState.Idle);
        }
    }

    public override void StateExit()
    {
        m_Ghoul.Anim.SetBool("Walk", false);
    }

    private void FindWayPoints()
    {
        GameObject wayPoints = m_Ghoul.transform.parent.gameObject;

        foreach(Transform wayTrans in wayPoints.transform)
        {
            WayPointList.Add(wayTrans); 
        }

        int randomPoint = UnityEngine.Random.Range(0, WayPointList.Count);

        m_Ghoul.Agent.SetDestination(WayPointList[randomPoint].position);

        m_Ghoul.Anim.SetBool("Walk", true);
    }

}

public class GhoulTraceState : GhoulState
{
    public GhoulTraceState(Ghoul ghoul) : base(ghoul)
    {
    }
}

public class GhoulAttackState : GhoulState
{
    public GhoulAttackState(Ghoul ghoul) : base(ghoul)
    {
    }
}