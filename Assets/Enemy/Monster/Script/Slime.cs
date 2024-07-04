using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    [Header("MeshRenderer")]
    [SerializeField] private MeshRenderer m_SlimeMeshRenderer;

    protected override void Start()
    {
        base.Start();
        InitStateMachine();
        InitMaterial();
        InitSlime();
    }

    private void InitStateMachine()
    {
        m_monsterStateMachine = gameObject.AddComponent<MonsterStateMachine>();
        
    }

    private void InitMaterial()
    {
        m_copyMaterial = Instantiate(m_originalMaterial);
        m_SlimeMeshRenderer.material = m_copyMaterial;
        m_saveColor = m_copyMaterial.GetColor("_Color");
    }


    private void InitSlime()
    {
        int id = (int)MonsterData.Slime;
        m_monsterHealth = DataManager.Instance.GetMonsterData(id).MonsterHp;
        m_monsterAttackPower = DataManager.Instance.GetMonsterData(id).MonsterAttackPower;
        m_monsterSpeed = DataManager.Instance.GetMonsterData(id).MonsterSpeed;
    }

    public override void TakeDamage(float damage)
    {
        

    }
}

public abstract class SlimeState : Monster_BaseState
{
    protected Slime m_Slime;

    public SlimeState(Slime slime)
    {
        m_Slime = slime;
    }
}

public class SlimeIdleState : SlimeState
{
    public SlimeIdleState(Slime slime) : base(slime)
    {

    }

}

public class SlimeMoveState : SlimeState
{
    public SlimeMoveState(Slime slime) : base(slime)
    {

    }
}
