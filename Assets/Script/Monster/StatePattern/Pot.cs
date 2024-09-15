
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Pot : Monster
{
    [Header("MeshRenderer")]
    [SerializeField] private MeshRenderer m_renderer;

    [Header("MeshRenderer_Lid")]
    [SerializeField] private MeshRenderer m_lid;

    [Header("HideMesh")]
    [SerializeField] GameObject m_hideMesh;

    [Header("ExplosionParticle")]
    [SerializeField] private GameObject m_particle;

    [Header("SoulPosition")]
    [SerializeField] private GameObject m_DropSoulPosition;

    private void Awake()
    {
        InitStateMachine();
    }

    protected override void Start()
    {
        InitPot();
        InitMaterial();
    }

    private void InitPot()
    {
        //_data = MonsterManager.Instance.GetMonsterData(MonsterType.Pot);

        //m_monsterHealth = _data._health;
        //m_monsterAttackPower = _data._attackPower;
        //m_monsterSpeed = _data._speed;
        m_searchCollider = GetComponent<SphereCollider>();
        m_monsterAgent = GetComponent<NavMeshAgent>();
        m_monsterAnim = GetComponent<Animator>();
        m_monsterRigid = GetComponent<Rigidbody>();
        m_player = GameManager.Instance.Player;
        m_monsterAgent.speed = m_monsterSpeed;
        m_explosionParticle = Instantiate(m_particle, transform);
        m_explosionParticle.SetActive(false);
    }

    

    private void InitStateMachine()
    {
        m_monsterStateMachine = gameObject.AddComponent<MonsterStateMachine>();
        m_monsterStateMachine.AddState(MonsterState.Idle, new PotIdleState(this));
        m_monsterStateMachine.AddState(MonsterState.Hide, new PotHideState(this));
        m_monsterStateMachine.AddState(MonsterState.Trace, new PotTraceState(this));
        m_monsterStateMachine.AddState(MonsterState.SelfDestruction, new PotSelfDestructionState(this));
    }

    private void InitMaterial()
    {
        m_copyMaterial = Instantiate(m_originalMaterial);
        m_renderer.material = m_copyMaterial;
        m_saveColor = m_copyMaterial.GetColor("_Color");

        m_LidMaterial = Instantiate(m_originalMaterial);
        m_lid.material = m_copyMaterial;
        m_LidColor = m_copyMaterial.GetColor("_Color");
    }

    public MonsterStateMachine State { get { return m_monsterStateMachine; } }
    public Material CopyMaterial { get { return m_copyMaterial; } }
    public NavMeshAgent Agent { get { return m_monsterAgent; } }    
    public Animator Anim { get { return m_monsterAnim; } }  
    public Rigidbody Rigidbody { get { return m_monsterRigid; } }   
    public GameObject Player { get { return m_player; } }
    public GameObject HideMesh { get { return m_hideMesh; } }
    public SphereCollider SearchCollider { get { return m_searchCollider; } }
    public GameObject ExParticle { get { return m_explosionParticle; } }    
    public int AttackPower { get { return m_monsterAttackPower; } } 

    private SphereCollider m_searchCollider;
    private GameObject m_explosionParticle;
    private Material m_LidMaterial;
    private Color m_LidColor;

    public override void TakeDamage(float damage)
    {
        m_monsterHealth -= (int)damage;

        SkillManager.Instance.AddSkillCount();

        if(m_monsterHealth <= 0)
        {
            Die();
            StartCoroutine(Delete_Lid());
        }
        else
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
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
        m_monsterAnim.SetTrigger("Explosion");
        StartCoroutine(Die(5f, 0.5f, 0.001f));
    }

    public IEnumerator Delete_Lid()
    {
        float timer = 5f;

        float colorMaxValue = 0.5f;

        float reductionAmount = 0.001f;

        while (timer > 0f)
        {
            colorMaxValue -= reductionAmount;
            m_copyMaterial.SetFloat("_Float", colorMaxValue);
            yield return null;
            timer -= Time.deltaTime;
        }
    }
}

public class PotState : Monster_BaseState
{
    protected Pot m_pot;
    public PotState(Pot pot)
    {
        m_pot = pot;
    }
}

public class PotHideState : PotState
{
    public PotHideState(Pot pot) : base(pot) { }

    public override void StateEnter()
    {
        m_pot.Anim.SetBool("Hide", true);
        m_pot.SearchCollider.enabled = true;
        m_pot.HideMesh.SetActive(false);
    }

    public override void StateExit()
    {
        m_pot.Anim.SetBool("Hide", false);
        m_pot.SearchCollider.enabled = false;
        m_pot.HideMesh.SetActive(true);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_pot.State.ChangeState(MonsterState.Trace);
        }
    }

}

public class PotIdleState : PotState
{
    public PotIdleState(Pot pot) : base(pot) { }

    private bool isHide = true;

    public override void StateEnter()
    {
        if (isHide)
        {
            isHide = false;
            m_pot.State.ChangeState(MonsterState.Hide);
        }    
    }

}

public class PotTraceState : PotState
{
    public PotTraceState(Pot pot) : base(pot) { }

    public override void StateEnter()
    {
        m_pot.Anim.SetBool("Walk", true);
        m_pot.Agent.stoppingDistance = 1;
        m_pot.Agent.SetDestination(m_pot.Player.transform.position);
    }

    public override void StateUpdate()
    {
        m_pot.Agent.SetDestination(m_pot.Player.transform.position);
    }

    public override void StateExit()
    {
        m_pot.Anim.SetBool("Walk", false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if (hit != null)
            {
                hit.TakeDamage(m_pot.AttackPower);
            }

            m_pot.State.ChangeState(MonsterState.SelfDestruction);
        }
    }
 
}

public class PotSelfDestructionState : PotState
{
    public PotSelfDestructionState(Pot pot) : base(pot) { }

    public override void StateEnter()
    {
        m_pot.HideMesh.SetActive(false);

        m_pot.Anim.SetTrigger("Explosion");

        m_pot.gameObject.layer = LayerMask.NameToLayer("DeadMonster");

        m_pot.ExParticle.SetActive(true);

        m_pot.Agent.isStopped = true;

        m_pot.Rigidbody.isKinematic = true;

        m_pot.StartCoroutine(Explosion(5f, 0.5f, 0.003f));
        m_pot.Delete_Lid();
    }

    private IEnumerator Explosion(float timer, float colorMaxValue, float reductionAmount)
    {
        while (timer > 0f)
        {
            colorMaxValue -= reductionAmount;
            m_pot.CopyMaterial.SetFloat("_Float", colorMaxValue);
            yield return null;
            timer -= Time.deltaTime;
        }

        m_pot.gameObject.SetActive(false);
    }

    
}