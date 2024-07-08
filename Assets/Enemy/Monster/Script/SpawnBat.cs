using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class SpawnBat : Monster
{
    [Header("AttackArea")]
    [SerializeField] private GameObject m_AttakArea;

    [Header("SoulPosition")]
    [SerializeField] private GameObject m_DropSoulPosition;

    protected override void Start()
    {
        base.Start();
        InitBat();
        InitMaterial();
        InitStateMachine();
    }

    private void InitBat()
    {
        m_monsterHealth = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterHp;
        m_monsterAttackPower = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterAttackPower;
        m_monsterSpeed = DataManager.Instance.GetMonsterData((int)MonsterData.Bat).MonsterSpeed;
        m_monsterAgent.speed = m_monsterSpeed;
        m_hitCollider = m_AttakArea.GetComponent<BoxCollider>();
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
        m_monsterStateMachine.AddState(MonsterState.Idle, new SpawnBat_IdleState(this));
        m_monsterStateMachine.AddState(MonsterState.Trace, new SpawnBat_TraceState(this));
        m_monsterStateMachine.AddState(MonsterState.Attack, new SpawnBat_AttackState(this));
    }

    public MonsterStateMachine State { get { return m_monsterStateMachine; } }
    public NavMeshAgent Agent { get { return m_monsterAgent; } }
    public Animator Anim { get { return m_monsterAnim; } }
    public GameObject Player { get { return m_player; } }
    public GameObject HitArea { get { return m_AttakArea; } }
    public int AttackPower { get { return m_monsterAttackPower; } }
    public bool IsDie
    {
        get { return isDie; }
        set { isDie = value; }
    }

    private BoxCollider m_hitCollider;
    private bool isDie = false;


    public override void TakeDamage(float damage)
    {
        m_monsterHealth -= (int)damage;

        SkillManager.Instance.AddSkillCount();

        if (m_monsterHealth <= 0)
        {
            isDie = true;
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

        GameObject soul = PoolManager.Instance.GetSoul();
        DropSoul soulComponent = soul.GetComponent<DropSoul>();
        soul.transform.SetParent(m_DropSoulPosition.transform);
        soul.transform.localPosition = Vector3.zero;
        soul.SetActive(true);
        soulComponent.StartCoroutine(soulComponent.Fly());
        soul.transform.parent = null;

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

    public void OnCollider()
    {
        m_hitCollider.enabled = true;
    }

    public void OffCollider()
    {
        m_hitCollider.enabled = false;
    }
}

public abstract class SpawnBatState : Monster_BaseState
{
    protected SpawnBat m_Bat;
    public SpawnBatState(SpawnBat bat)
    {
        m_Bat = bat;
    }
}

public class SpawnBat_IdleState : SpawnBatState
{
    public SpawnBat_IdleState(SpawnBat bat) : base(bat) { }

    public override void StateEnter()
    {
        m_Bat.State.ChangeState(MonsterState.Trace);
    }

}

public class SpawnBat_TraceState : SpawnBatState
{
    public SpawnBat_TraceState(SpawnBat bat) : base(bat) { }

    private float m_attackDistance = 3.0f;

    private WaitForSeconds m_attackCoolTime = new WaitForSeconds(6.0f);

    private bool isAttack = true;

    public override void StateEnter()
    {
        m_Bat.Agent.SetDestination(m_Bat.Player.transform.position);
    }

    public override void StateUpdate()
    {
        if (Vector3.Distance(m_Bat.transform.position, m_Bat.Player.transform.position) <= m_attackDistance && isAttack)
        {
            isAttack = false;

            m_Bat.State.ChangeState(MonsterState.Attack);

            m_Bat.StartCoroutine(AttackCoolTime());
        }
        else
            m_Bat.Agent.SetDestination(m_Bat.Player.transform.position);
    }

    public override void StateExit()
    {
        m_Bat.Agent.SetDestination(m_Bat.transform.position);
    }

    private IEnumerator AttackCoolTime()
    {
        yield return m_attackCoolTime;

        isAttack = true;
    }

}

public class SpawnBat_AttackState : SpawnBatState
{
    public SpawnBat_AttackState(SpawnBat bat) : base(bat) { }

    public override void StateEnter()
    {
        m_Bat.Agent.updateRotation = false;

        SpawnBatAttackArea hitArea = m_Bat.HitArea.GetComponent<SpawnBatAttackArea>();

        hitArea.SetAttackPower(m_Bat.AttackPower);

        if(Vector3.Distance(m_Bat.transform.position, m_Bat.Player.transform.position) > 1f && !m_Bat.IsDie)
        {
            m_Bat.StartCoroutine(AttackMovement());
        }
    }

    public override void StateUpdate()
    {
        m_Bat.Anim.SetTrigger("Attack");

        AttackRotation();
    }

    public override void StateExit()
    {
        m_Bat.Agent.updateRotation = true;
    }

    private void AttackRotation()
    {
        Vector3 targetPos = (m_Bat.Player.transform.position - m_Bat.transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(targetPos);

        m_Bat.transform.rotation = Quaternion.Slerp(m_Bat.transform.rotation, rotation, 3.0f * Time.deltaTime);
    }

    private IEnumerator AttackMovement()
    {
        float moveDistance = 2.0f;
        Vector3 direction = m_Bat.transform.forward;

        float moveTime = 0.5f;
        float startTime = Time.time;
        
        Vector3 startPosition = m_Bat.transform.position;
        Vector3 endPosition = startPosition + (direction * moveDistance);

        while (Time.time < startTime + moveTime)
        {
            if (m_Bat.IsDie)
            {
                yield break;
            }

            float t = (Time.time - startTime) / moveTime;
            m_Bat.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        m_Bat.transform.position = endPosition;
    }

}

