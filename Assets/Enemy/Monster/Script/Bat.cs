using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Bat : Monster
{
    protected override void Start()
    {
        base.Start();
        InitStateMachine();
        InitBat();
        InitMaterial();
    }

    private void InitBat()
    {
        m_monsterHealth = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterHp;
        m_monsterAttackPower = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterAttackPower;
        m_monsterSpeed = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterSpeed;
        m_monsterAgent.speed = m_monsterSpeed;
    }

    private void InitMaterial()
    {
        m_copyMaterial = Instantiate(m_originalMaterial);
        m_skinnedMeshRenderer.material = m_copyMaterial;
        m_saveColor = m_copyMaterial.GetColor("_Color");
    }

    private void InitStateMachine()
    {
        m_monsterStateMachine = gameObject.AddComponent<MonsterStateMachine>();
        m_monsterStateMachine.AddState(MonsterState.Idle, new IdleState(this));
        m_monsterStateMachine.AddState(MonsterState.Move, new MoveState(this));
    }

    public MonsterStateMachine State { get { return m_monsterStateMachine; } }
    public NavMeshAgent Agent { get { return m_monsterAgent; } }
    public Animator Anim { get { return m_monsterAnim; } }
    

    public override void TakeDamage(float damage)
    {
        m_monsterHealth -= (int)damage;

        SkillManager.Instance.AddSkillCount();

        if(m_monsterHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
            
    }

    private void Die()
    {
        if (isSpawn)
        {
            GimikManager.Instance.UnRegisterMonster(this.gameObject);

            isSpawn = false;
        }

        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        m_monsterAgent.SetDestination(transform.position);
        m_monsterAgent.isStopped = true;
        m_monsterRigid.isKinematic = true;
        m_monsterAnim.SetTrigger("Die");
        StartCoroutine(Die(5f, 0.5f, 0.003f));
    }

    public void IsSpawn(bool isSpawn)
    {
        this.isSpawn = isSpawn;
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
    }

    public override void StateExit()
    {
        m_Bat.Anim.SetBool("Move", false);
    }
}




