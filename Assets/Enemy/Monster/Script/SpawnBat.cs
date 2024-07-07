using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnBat : Monster
{
    protected override void Start()
    {
        base.Start();
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
        
    }

    public MonsterStateMachine State { get { return m_monsterStateMachine; } }
    public NavMeshAgent Agent { get { return m_monsterAgent; } }
    public Animator Anim { get { return m_monsterAnim; } }


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

public abstract class SpawnBatState : Monster_BaseState
{
    protected SpawnBat m_Bat;
    public SpawnBatState(SpawnBat bat)
    {
        m_Bat = bat;
    }
}

public class SpawnBat_TraceState : SpawnBatState
{
    public SpawnBat_TraceState(SpawnBat bat) : base(bat) { }


    
}

