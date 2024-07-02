using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum MonsterData
{
    Bat = 1,
    Mage = 3,
    Pot,
}

public abstract class Monster : MonoBehaviour, IDamged
{
    [Header("SkinnedMeshRenderer")]
    [SerializeField] protected SkinnedMeshRenderer m_skinnedMeshRenderer;
    [Header("Material")]
    [SerializeField] protected Material m_originalMaterial;

    protected MonsterStateMachine m_monsterStateMachine;
    protected NavMeshAgent m_monsterAgent;
    protected Animator m_monsterAnim;
    protected Rigidbody m_monsterRigid;
    protected Material m_copyMaterial;
    protected GameObject m_player;
    protected Color m_saveColor;

    [SerializeField]
    protected int m_monsterHealth;
    protected int m_monsterAttackPower;
    protected float m_monsterSpeed;

    public abstract void TakeDamage(float damage);
    
    protected virtual void Start()
    {
        m_monsterAgent = GetComponent<NavMeshAgent>();
        m_monsterAnim = GetComponent<Animator>();
        m_monsterRigid = GetComponent<Rigidbody>();
        m_player = GameManager.Instance.Player;
    }


    protected IEnumerator IntensityChange(float powValue1, float powValue2)
    {
        Color color = m_copyMaterial.GetColor("_Color");
        Color newColor = color * Mathf.Pow(powValue1, powValue2);
        m_copyMaterial.SetColor("_Color", newColor);

        yield return new WaitForSeconds(0.1f);

        m_copyMaterial.SetColor("_Color", m_saveColor);
    }

    protected IEnumerator Die(float timer, float colorMaxValue, float reductionAmount)
    {
        while(timer > 0f)
        {
            colorMaxValue -= reductionAmount;
            m_copyMaterial.SetFloat("_Float", colorMaxValue);
            yield return null;
            timer -= Time.deltaTime;
        }

        gameObject.SetActive(false);
    }
}



