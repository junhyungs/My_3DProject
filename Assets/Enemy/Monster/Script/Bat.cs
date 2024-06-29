using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Bat : Monster
{
    protected override void Start()
    {
        base.Start();
        InitStateMachine();
        InitBat();
    }

    private void InitBat()
    {
        m_monsterHealth = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterHp;
        m_monsterAttackPower = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterAttackPower;
        m_monsterSpeed = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterSpeed;
    }

    private void InitStateMachine()
    {
        m_monsterStateMachine = gameObject.AddComponent<MonsterStateMachine>();
        m_monsterStateMachine.AddState(MonsterState.Idle, new IdleState(this));
        m_monsterStateMachine.AddState(MonsterState.Move, new MoveState(this));
        m_monsterStateMachine.AddState(MonsterState.Trace, new TraceState(this));
        m_monsterStateMachine.AddState(MonsterState.Attack, new AttackState(this));
    }

    public MonsterStateMachine State { get { return m_monsterStateMachine; } }
    public NavMeshAgent Agent { get { return m_monsterAgent; } }
    public Animator Anim { get { return m_monsterAnim; } }
    public GameObject Player { get { return m_player; } }

    public void ChangerTrace()
    {
        m_monsterStateMachine.ChangeState(MonsterState.Trace);
    }

    public override void TakeDamage(int damage)
    {
        m_monsterHealth -= damage;

        m_monsterAnim.SetTrigger("Hit");
        
        if(m_monsterHealth <= 0)
        {
            gameObject.SetActive(false);    
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
            Debug.Log("검 콜라이더");
    }

}

public abstract class BatState : Monster_BaseState
{
    protected Bat m_Bat;
    public BatState(Bat bat)
    {
        m_Bat = bat;
    }
}

public class IdleState : BatState
{
    public IdleState(Bat bat) : base(bat) { }

    private float m_idleTime;

    public override void StateEnter()
    {
        m_Bat.StartCoroutine(ChangeTimer());
    }

    public override void StateUpdate()
    {
        TargetToPlayer();
    }

    public override void StateExit()
    {
        m_idleTime = 0f;
    }

    private IEnumerator ChangeTimer()
    {
        while (true)
        {
            m_idleTime += Time.deltaTime;

            yield return null;

            if (m_idleTime >= 0.5f)
            {
                m_Bat.State.ChangeState(MonsterState.Move);
                yield break;
            }
        }
    }

    private void TargetToPlayer()
    {
        if(Vector3.Distance(m_Bat.Player.transform.position, m_Bat.transform.position) <= 7.0f)
        {
            m_Bat.StopCoroutine(ChangeTimer()); 
            m_Bat.State.ChangeState(MonsterState.Trace);
        }
    }
}

public class MoveState : BatState
{
    public MoveState(Bat bat) : base(bat) { }

    private List<Transform> WayPointList = new List<Transform>();

    public override void StateEnter()
    {
        GameObject wayPoints = m_Bat.transform.parent.gameObject;

        foreach (Transform wayTransform in wayPoints.transform)
        {
            WayPointList.Add(wayTransform);
        }

        int randomPoint = Random.Range(0, WayPointList.Count);

        m_Bat.Agent.SetDestination(WayPointList[randomPoint].position);
       
        m_Bat.Anim.SetBool("Move", true);
    }

    public override void StateUpdate()
    {
        if(m_Bat.Agent.remainingDistance <= m_Bat.Agent.stoppingDistance)
        {
            m_Bat.State.ChangeState(MonsterState.Idle);
        }
        
        TargetToPlayer();
    }

    public override void StateExit()
    {
        m_Bat.Anim.SetBool("Move", false);
    }

    private void TargetToPlayer()
    {
        if(Vector3.Distance(m_Bat.Player.transform.position, m_Bat.transform.position) <= 7.0f)
        {
            m_Bat.State.ChangeState(MonsterState.Trace);
        }
    }
}

public class TraceState : BatState
{
    public TraceState(Bat bat) : base(bat) { }

    public override void StateEnter()
    {
        m_Bat.Agent.speed = 7.0f;
        m_Bat.Anim.SetBool("Move", true);
        m_Bat.Anim.ResetTrigger("Attack");
        
    }

    public override void StateUpdate()
    {
        Return();
        Attack();
    }

    public override void StateExit()
    {
        m_Bat.Agent.speed = 5.0f;
        m_Bat.Anim.SetBool("Move", false);
    }

    private void Return()
    {
        if(Vector3.Distance(m_Bat.Player.transform.position, m_Bat.transform.position) > 7.0f)
        {
            m_Bat.State.ChangeState(MonsterState.Move);
        }
        else
        {
            m_Bat.Agent.SetDestination(m_Bat.Player.transform.position);
        }
    }

    private void Attack()
    {
        if(m_Bat.Agent.remainingDistance <= m_Bat.Agent.stoppingDistance)
        {
            m_Bat.State.ChangeState(MonsterState.Attack);
        }
    }
}

public class AttackState : BatState
{
    public AttackState(Bat bat) : base(bat) { }

    public override void StateEnter()
    {
        m_Bat.Anim.SetTrigger("Attack");
    }
}


