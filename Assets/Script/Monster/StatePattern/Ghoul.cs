
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.AI;




public class Ghoul : Monster, IDisableArrow
{
    [Header("FirePosition")]
    [SerializeField] private GameObject m_FirePosition;

    [Header("BowMeshRenderer")]
    [SerializeField] private MeshRenderer m_bowRenderer;

    [Header("SoulPosition")]
    [SerializeField] private GameObject m_DropSoulPosition;

    private void OnEnable()
    {
        EventManager.Instance.RegisterDisableGhoulArrow(this);
    }

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
        _data = MonsterManager.Instance.GetMonsterData(MonsterType.Ghoul);

        m_monsterHealth = _data._health;
        m_monsterAttackPower = _data._attackPower;
        m_monsterSpeed = _data._speed;
        m_monsterAgent.speed = m_monsterSpeed;
        m_startPosition = transform.position;
        isAlive = true;
    }

    private void InitMaterial()
    {
        m_copyMaterial = Instantiate(m_originalMaterial);
        m_skinnedMeshRenderer.material = m_copyMaterial;
        m_saveColor = m_copyMaterial.GetColor("_Color");

        m_bowRenderer.material = m_copyMaterial;
    }

    public override void TakeDamage(float damage)
    {
        m_monsterHealth -= (int)damage;

        SkillManager.Instance.AddSkillCount();

        if (m_monsterHealth <= 0)
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

        GameObject soul = PoolManager.Instance.GetSoul();
        DropSoul soulComponent = soul.GetComponent<DropSoul>();
        soul.transform.SetParent(m_DropSoulPosition.transform);
        soul.transform.localPosition = Vector3.zero;
        soul.SetActive(true);
        soulComponent.StartCoroutine(soulComponent.Fly());
        soul.transform.parent = null;

        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        isAlive = false;

        m_monsterAgent.enabled = false;
        m_monsterRigid.isKinematic = true;
        OnDisableHendeler?.Invoke();

        m_monsterAnim.SetTrigger("Die");
        StartCoroutine(Die(5f, 0.5f, 0.003f));
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

    public Vector3 StartPosition
    {
        get { return m_startPosition; }
    }

    public bool IsAction
    {
        get { return isAction; }
        set { isAction = value; }
    }

    public bool IsAlive
    {
        get { return isAlive; } 
    }

    private Vector3 m_startPosition;
    private bool isAction;
    private bool isAlive;
    private Action OnDisableHendeler;

    public void Arrow()
    {
        if (isAlive)
        {
            GameObject arrow = PoolManager.Instance.GetMonsterArrow();

            GhoulArrow arrowComponent = arrow.GetComponent<GhoulArrow>();
            arrowComponent.IsFire(false);
            arrowComponent.SetAttackPower(m_monsterAttackPower);
            arrow.transform.SetParent(m_FirePosition.transform);
            arrow.transform.localPosition = Vector3.zero;
            arrow.transform.rotation = m_FirePosition.transform.rotation;
        }
    }

    public void ArrowFire()
    {
        if(m_FirePosition.transform.childCount != 0 && isAlive)
        {
            GameObject arrow = m_FirePosition.transform.GetChild(0).gameObject;
            GhoulArrow arrowComponent = arrow.GetComponent<GhoulArrow>();
            arrowComponent.IsFire(true);
            arrow.transform.parent = null;
        }
       
    }

    public void OnDisableArrow(Action callBack)
    {
        OnDisableHendeler += callBack;
    }

    public void IsSpawn(bool isSpawn)
    {
        this.isSpawn = isSpawn;
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
    public GhoulIdleState(Ghoul ghoul) : base(ghoul) { }
    
    private float m_traceDistance = 20.0f;

    public override void StateUpdate()
    {
        TargetToPlayer();
    }

    private void TargetToPlayer()
    {
        if(Vector3.Distance(m_Ghoul.transform.position, m_Ghoul.Player.transform.position) <= m_traceDistance)
        {
            m_Ghoul.State.ChangeState(MonsterState.Trace);
        }
    }

}

public class GhoulMoveState : GhoulState
{
    public GhoulMoveState(Ghoul ghoul) : base(ghoul) { }

    public override void StateEnter()
    {
        m_Ghoul.Anim.SetBool("Walk", true);

        m_Ghoul.gameObject.layer = LayerMask.NameToLayer("DeadMonster");

        m_Ghoul.Agent.stoppingDistance = 0;

        m_Ghoul.Agent.SetDestination(m_Ghoul.StartPosition);
    }

    public override void StateUpdate()
    {
        if (m_Ghoul.Agent.remainingDistance <= m_Ghoul.Agent.stoppingDistance)
        {
            m_Ghoul.Anim.SetBool("Walk", false);

            m_Ghoul.State.ChangeState(MonsterState.Idle);
        }
    }

    public override void StateExit()
    {
        m_Ghoul.gameObject.layer = LayerMask.NameToLayer("Monster");

        m_Ghoul.Agent.stoppingDistance = 10f;
    }
  
}

public class GhoulTraceState : GhoulState
{
    public GhoulTraceState(Ghoul ghoul) : base(ghoul) { }

    private float m_returnMoveDistance = 20.0f;

    public override void StateEnter()
    {
        m_Ghoul.Anim.SetBool("TraceWalk", true);

        m_Ghoul.Agent.speed += 2;

        m_Ghoul.Agent.SetDestination(m_Ghoul.Player.transform.position);
    }

    public override void StateUpdate()
    {
        if (!m_Ghoul.IsAlive)
            return;

        ReturnMoveState();
        ChangeAttackState();
    }

    public override void StateExit()
    {
        m_Ghoul.Agent.speed -= 2;
    }

    private void ChangeAttackState()
    {
        if(m_Ghoul.Agent.remainingDistance <= m_Ghoul.Agent.stoppingDistance)
        {
            m_Ghoul.Anim.SetBool("TraceWalk", false);

            m_Ghoul.State.ChangeState(MonsterState.Attack);
        }
    }

    private void ReturnMoveState()
    {
        if(Vector3.Distance(m_Ghoul.transform.position, m_Ghoul.Player.transform.position) > m_returnMoveDistance)
        {
            m_Ghoul.Agent.SetDestination(m_Ghoul.transform.position);

            m_Ghoul.Anim.SetBool("TraceWalk", false);

            m_Ghoul.State.ChangeState(MonsterState.Move);
        }
        else
            m_Ghoul.Agent.SetDestination(m_Ghoul.Player.transform.position);
    }

}

public class GhoulAttackState : GhoulState
{
    public GhoulAttackState(Ghoul ghoul) : base(ghoul) { }

    private float m_returnTraceDistance = 10.0f;

    public override void StateEnter()
    {
        m_Ghoul.Agent.enabled = false;
    }

    public override void StateUpdate()
    {
        Attack();
        RotationToPlayer();
    }    

    public override void StateExit()
    {
        m_Ghoul.Anim.ResetTrigger("Attack");

        m_Ghoul.Agent.enabled = true;
    }

    private void Attack()
    {
        if (Vector3.Distance(m_Ghoul.transform.position, m_Ghoul.Player.transform.position) <= m_returnTraceDistance)
        {
            m_Ghoul.Anim.SetTrigger("Attack");
        }
        else
        {
            if (!m_Ghoul.IsAction && m_Ghoul.IsAlive)
            {
                m_Ghoul.State.ChangeState(MonsterState.Trace);
            }
        }
       
    }

    private void RotationToPlayer()
    {
        if (m_Ghoul.IsAlive)
        {
            Vector3 rotDir = (m_Ghoul.Player.transform.position - m_Ghoul.transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(rotDir);

            m_Ghoul.transform.rotation = Quaternion.Slerp(m_Ghoul.transform.rotation, rotation, 2.0f * Time.deltaTime);
        }
    }

}
