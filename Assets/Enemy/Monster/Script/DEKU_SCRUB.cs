using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DEKU_SCRUB : Monster
{
    [Header("FirePosition")]
    [SerializeField] private GameObject m_FirePosition;

    [Header("SoulPosition")]
    [SerializeField] private GameObject m_DropSoulPosition;

    protected override void Start()
    {
        base.Start();
        InitDeku();
        InitStateMachine();
        InitMaterial();
    }

    private void InitDeku()
    {
        _data = MonsterManager.Instance.GetMonsterData(MonsterType.Deku);

        m_monsterHealth = _data._health;
        m_monsterAttackPower = _data._attackPower;
        m_monsterSpeed = _data._speed;
        m_monsterAgent.speed = m_monsterSpeed;
        m_searchCollider = GetComponent<SphereCollider>();
        m_startPosition = transform.position;
        isDie = false;
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
        m_monsterStateMachine.AddState(MonsterState.Hide, new Deku_HideState(this));
        m_monsterStateMachine.AddState(MonsterState.Idle, new Deku_IdleState(this));
        m_monsterStateMachine.AddState(MonsterState.Move, new Deku_MoveState(this));
        m_monsterStateMachine.AddState(MonsterState.Trace, new Deku_TraceState(this));
        m_monsterStateMachine.AddState(MonsterState.Attack, new Deku_AttackState(this));
        m_monsterStateMachine.StartState(MonsterState.Hide);
    }

    public MonsterStateMachine State { get { return m_monsterStateMachine; } }
    public Animator Anim { get { return m_monsterAnim; } }
    public NavMeshAgent Agent { get { return m_monsterAgent; } }
    public GameObject Player { get { return m_player; } }   
    public Vector3 StartPosition { get { return m_startPosition; } }    
    public Rigidbody DekuRigid { get { return m_monsterRigid; } }
    public SphereCollider SearchCollider { get { return m_searchCollider; } }
    public bool IsAction
    {
        get { return isAction; }
        set { isAction = value; }
    }
    public bool IsDie
    {
        get { return isDie; }
    }

    private SphereCollider m_searchCollider;
    private Vector3 m_startPosition;
    private bool isAction;
    private bool isDie;
    

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
            StartCoroutine(IntensityChange(2f, 3f));
    }

    private void Die()
    {
        GameObject soul = PoolManager.Instance.GetSoul();
        DropSoul soulComponent = soul.GetComponent<DropSoul>();
        soul.transform.SetParent(m_DropSoulPosition.transform);
        soul.transform.localPosition = Vector3.zero;
        soul.SetActive(true);
        soulComponent.StartCoroutine(soulComponent.Fly());
        soul.transform.parent = null;

        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        m_monsterAgent.isStopped = true;
        m_monsterRigid.isKinematic = true;
        m_monsterAnim.SetTrigger("Die");
        StartCoroutine(Die(5f, 0.5f, 0.001f));
    }

    public void DekuProjectile()
    {
        GameObject bullet = PoolManager.Instance.GetDekuProjectile();

        bullet.transform.position = m_FirePosition.transform.position;
        bullet.transform.rotation = m_FirePosition.transform.rotation;
        DekuBullet bulletComponent = bullet.GetComponent<DekuBullet>();
        bulletComponent.IsFire(true);
    }

}

public abstract class DekuState : Monster_BaseState
{
    protected DEKU_SCRUB m_Deku;

    public DekuState(DEKU_SCRUB deku)
    {
        m_Deku = deku;
    }
}

public class Deku_HideState : DekuState
{
    public Deku_HideState(DEKU_SCRUB deku) : base(deku) { }

    public override void StateEnter()
    {
        m_Deku.SearchCollider.enabled = true;

        m_Deku.Anim.SetBool("Hide", true);
    }

    public override void StateExit()
    {
        m_Deku.SearchCollider.enabled = false;

        m_Deku.Anim.SetBool("Hide", false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_Deku.State.ChangeState(MonsterState.Trace);
        }
    }
}

public class Deku_IdleState : DekuState
{
    public Deku_IdleState(DEKU_SCRUB deku) : base(deku) { }

    private float m_sphereRidius = 3.0f;

    public override void StateEnter()
    {        
        if (SearchPlayer())
        {
            m_Deku.StartCoroutine(Hide());
        }
        else
            m_Deku.State.ChangeState(MonsterState.Trace);
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(1.0f);

        m_Deku.State.ChangeState(MonsterState.Hide);
    }

    private bool SearchPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(m_Deku.transform.position, m_sphereRidius, LayerMask.GetMask("Player"));

        if (colliders.Length == 0)
            return true;
        else
            return false;
    }

}

public class Deku_MoveState : DekuState
{
    public Deku_MoveState(DEKU_SCRUB deku) : base(deku) { }

    public override void StateEnter()
    {
        m_Deku.Anim.SetBool("Walk", true);

        m_Deku.Agent.SetDestination(m_Deku.StartPosition);
    }

    public override void StateUpdate()
    {
        if(m_Deku.Agent.remainingDistance <= m_Deku.Agent.stoppingDistance)
        {
            m_Deku.State.ChangeState(MonsterState.Idle);
        }
        else
            m_Deku.Agent.SetDestination(m_Deku.StartPosition);
    }

    public override void StateExit()
    {
        m_Deku.Anim.SetBool("Walk", false);

        m_Deku.Agent.SetDestination(m_Deku.transform.position);
    }

}

public class Deku_TraceState : DekuState
{
    public Deku_TraceState(DEKU_SCRUB deku) : base(deku) { }

    public override void StateEnter()
    {
        m_Deku.Anim.SetBool("Walk", true);
    }

    public override void StateUpdate()
    {
        if(m_Deku.Agent.remainingDistance <= m_Deku.Agent.stoppingDistance)
        {
            m_Deku.State.ChangeState(MonsterState.Attack);
        }

        m_Deku.Agent.SetDestination(m_Deku.Player.transform.position);
    }

    public override void StateExit()
    {
        m_Deku.Anim.SetBool("Walk", false);
    }

}

public class Deku_AttackState : DekuState
{
    public Deku_AttackState(DEKU_SCRUB deku) : base(deku) { }

    private Vector3 targetPosition;

    public override void StateEnter()
    {
        m_Deku.Agent.SetDestination(m_Deku.transform.position);
        m_Deku.Agent.updateRotation = false;
    }

    public override void StateUpdate()
    {
       
        if (Vector3.Distance(m_Deku.transform.position, m_Deku.Player.transform.position) > 5.0f && !m_Deku.IsAction)
        {
            m_Deku.State.ChangeState(MonsterState.Trace);
        }
        else
            m_Deku.Anim.SetTrigger("Attack");

        Rotation();
    }

    public override void StateExit()
    {
        m_Deku.Agent.updateRotation = true;
    }

    private void Rotation()
    {
        if (!m_Deku.IsDie)
        {
            targetPosition = m_Deku.Player.transform.position;

            Vector3 rotationDir = (targetPosition - m_Deku.transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(rotationDir);

            m_Deku.transform.rotation = Quaternion.Slerp(m_Deku.transform.rotation, lookRotation, Time.deltaTime * 3.0f);
        }
    }

}
