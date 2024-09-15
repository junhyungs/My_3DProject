using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mage : Monster, IDisableMagicBullet
{
    [Header("SoulPosition")]
    [SerializeField] private GameObject m_DropSoulPosition;

    [Header("FirePosition")]
    [SerializeField] private GameObject m_FirePosition;

    private void OnEnable()
    {
        EventManager.Instance.RegisterDisableMageBullet(this);
    }

    protected override void Start()
    {
        base.Start();
        InitMage();
        InitMaterial();
        InitStateMachine();
    }

    private void InitMage()
    {
        //_data = MonsterManager.Instance.GetMonsterData(MonsterType.Mage);

        //m_monsterHealth = _data._health;
        //m_monsterAttackPower = _data._attackPower;
        //m_monsterSpeed = _data._speed;
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
        m_monsterStateMachine.AddState(MonsterState.Idle, new MageIdleState(this));
        m_monsterStateMachine.AddState(MonsterState.Move, new MageMoveState(this));
        m_monsterStateMachine.AddState(MonsterState.Attack, new MageAttackState(this));
        m_monsterStateMachine.AddState(MonsterState.TelePort, new MageTelePortState(this));
    }

    public MonsterStateMachine State { get { return m_monsterStateMachine; } }
    public NavMeshAgent Agent { get { return m_monsterAgent; } }
    public Animator Anim { get { return m_monsterAnim; } }
    public GameObject Player { get { return m_player; } }
    public Material MageMaterial { get { return m_copyMaterial; } set { m_copyMaterial = value; } }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    
    private bool isDead = false;
    private Action OnDisableBulletHandler;

    public void MagicBullet()
    {
        GameObject magicBullet = PoolManager.Instance.GetMagicBullet();

        MagicBullet magicBulletComponent = magicBullet.GetComponent<MagicBullet>();
        magicBulletComponent.IsFire(false);
        magicBulletComponent.SetAttackPower(m_monsterAttackPower);
        magicBullet.transform.SetParent(m_FirePosition.transform);
        magicBullet.transform.localPosition = Vector3.zero;
        magicBullet.transform.localRotation = m_FirePosition.transform.localRotation;
        GameObject particle = magicBullet.transform.GetChild(0).gameObject;
        particle.SetActive(true);
    }

    public void MagicBulletfire()
    {
        if(m_FirePosition.transform.childCount != 0)
        {
            GameObject magicBullet = m_FirePosition.transform.GetChild(0).gameObject;
            MagicBullet magicBulletComponent = magicBullet.GetComponent<MagicBullet>();
            magicBulletComponent.IsFire(true);
            magicBullet.transform.parent = null;
        }
    }

    public override void TakeDamage(float damage)
    {
        m_monsterHealth -= (int)damage;

        SkillManager.Instance.AddSkillCount();

        if(m_monsterHealth <= 0)
        {
            isDead = true;

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
        OnDisableBulletHandler?.Invoke();
        StartCoroutine(Die(5f, 0.5f, 0.003f));
    }

    public void IsSpawn(bool isSpawn)
    {
        this.isSpawn = isSpawn;
    }

    public void OnDisableMagicBullet(Action callBack)
    {
        OnDisableBulletHandler += callBack;
    }
}

public abstract class MageState : Monster_BaseState
{
    protected Mage m_Mage;

    public MageState(Mage mage)
    {
        m_Mage = mage;
    }
}


public class MageIdleState : MageState
{
    public MageIdleState(Mage mage) : base(mage) { }

    public override void StateUpdate()
    {
        TraceToPlayer();
    }

    private void TraceToPlayer()
    {
        if(Vector3.Distance(m_Mage.Player.transform.position, m_Mage.transform.position) <= 10.0f)
        {
            m_Mage.State.ChangeState(MonsterState.TelePort);
        }
    }
}

public class MageMoveState : MageState
{
    public MageMoveState(Mage mage) : base(mage) { }
}

public class MageTelePortState : MageState
{
    public MageTelePortState(Mage mage) : base(mage) { }

    private float telePortRadius = 10.0f;

    public override void StateEnter()
    {
        m_Mage.gameObject.layer = LayerMask.NameToLayer("TelePort");

        if (!m_Mage.IsDead)
        {
            TelePort();
        }
    }

    public override void StateExit()
    {
        m_Mage.gameObject.layer = LayerMask.NameToLayer("Monster");
    }

    private void TelePort()
    {
        m_Mage.StartCoroutine(TelePort_In());
    }

    private IEnumerator TelePort_In()
    {
        m_Mage.Anim.SetTrigger("TelePort");

        float timer = 2.5f;

        float colorAmount = 0.005f;

        float colorMaxValue = 0.5f;

        while(timer > 0f)
        {
            colorMaxValue -= colorAmount;
            m_Mage.MageMaterial.SetFloat("_Float", colorMaxValue);
            yield return null;
            timer -= Time.deltaTime;
        }

        m_Mage.StartCoroutine(TelePort_Out());
    }

    private IEnumerator TelePort_Out()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * telePortRadius;

        randomDirection.y = 0;

        Vector3 telePortPosition = m_Mage.Player.transform.position + randomDirection;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(telePortPosition, out hit, telePortRadius, NavMesh.AllAreas))
        {
            m_Mage.Agent.Warp(hit.position);

            Vector3 rotateToPlayer = m_Mage.Player.transform.position - m_Mage.transform.position;

            rotateToPlayer.y = 0;

            if (rotateToPlayer != Vector3.zero)
            {
                m_Mage.transform.rotation = Quaternion.LookRotation(rotateToPlayer);
            }

        }

        yield return new WaitForSeconds(0.5f);

        m_Mage.Anim.SetBool("TelePort_In", true);

        float timer = 2.5f;

        float colorAmount = 0.005f;

        float colorMaxValue = -0.3f;

        while(timer > 0f)
        {
            colorMaxValue += colorAmount;
            m_Mage.MageMaterial.SetFloat("_Float", colorMaxValue);
            yield return null;
            timer -= Time.deltaTime;
        }

        m_Mage.Anim.SetBool("TelePort_In", false);

        m_Mage.State.ChangeState(MonsterState.Attack);
    }
}

public class MageAttackState : MageState
{
    public MageAttackState(Mage mage) : base(mage) { }

    public override void StateEnter()
    {
        m_Mage.Anim.SetTrigger("Attack");
    }

    public override void StateUpdate()
    {
        if (!m_Mage.IsDead)
        {
            RotateToPlayer();
            Move();
        }
    }

    public override void StateExit()
    {
        m_Mage.Agent.SetDestination(m_Mage.transform.position);
    }

    private void Move()
    {
        if(Vector3.Distance(m_Mage.Player.transform.position, m_Mage.transform.position) >= m_Mage.Agent.stoppingDistance)
        {
            m_Mage.Agent.SetDestination(m_Mage.Player.transform.position);
        }
    }

    private void RotateToPlayer()
    {
        Vector3 targetDir = (m_Mage.Player.transform.position - m_Mage.transform.position).normalized;

        targetDir.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        m_Mage.transform.rotation = Quaternion.Slerp(m_Mage.transform.rotation, targetRotation, 5f * Time.deltaTime);
    }


}