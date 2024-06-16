using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public enum PlayerSkill
{
    Bow,
    FireBall,
    Bomb,
    Hook
}

public class SkillManager : Singleton<SkillManager>
{
    [Header("PlayerPrefab")]
    [SerializeField] private GameObject m_player;

    private ISkill m_currentSkill;
    private PlayerSkill m_skillType;

    private Dictionary<PlayerSkill, SkillData> SkillDictionary = new Dictionary<PlayerSkill, SkillData>();
    private Dictionary<PlayerSkill, Skill> GetSkillDictionary = new Dictionary<PlayerSkill, Skill>();

    private void Awake()
    {
        InitSkill();
    }

    private void InitSkill()
    {
        SkillDictionary.Add(PlayerSkill.Bow, new SkillData(1.0f, 10.0f, 20.0f, 1));
        SkillDictionary.Add(PlayerSkill.FireBall, new SkillData(1.0f, 10.0f, 20.0f, 1));
        SkillDictionary.Add(PlayerSkill.Bomb, new SkillData(1.0f, 10.0f, 20.0f, 2));
        SkillDictionary.Add(PlayerSkill.Hook, new SkillData(1.0f, 10.0f, 20.0f, 0));

        m_skillType = PlayerSkill.Bow;
        SetSkill(m_skillType);
    }


    public void SetSkill(PlayerSkill skillName)
    {
        //if (!SkillDictionary.ContainsKey(skillName))
        //{
        //    return;
        //}

        m_skillType = skillName;

        Component component = gameObject.GetComponent<ISkill>() as Component;

        if (component != null)
        {
            Destroy(component);
        }

        switch (m_skillType)
        {
            case PlayerSkill.Bow:
                m_currentSkill = gameObject.AddComponent<Bow>();
                break;
            case PlayerSkill.FireBall:
                m_currentSkill = gameObject.AddComponent<FireBall>();
                break;
            case PlayerSkill.Bomb:
                m_currentSkill = gameObject.AddComponent<Bomb>();
                break;
            case PlayerSkill.Hook:
                m_currentSkill = gameObject.AddComponent<Hook>();
                break;
        }

    }

    public void AddSkill(PlayerSkill skillName, Skill skill)
    {
        GetSkillDictionary.Add(skillName, skill);
    }

    public SkillData GetSkillData(PlayerSkill skill)
    {
        return SkillDictionary[skill];
    }

    public void UseSkill()
    {
        m_currentSkill.UseSkill();
    }
}

public struct SkillData
{
    public float m_attackPower { get; }
    public float m_projectileSpeed { get; }
    public float m_projectileRange { get; }
    public int m_cost { get; }
    public SkillData(float attackPower,float projectileSpeed, float projectileRange, int cost)
    {
        m_attackPower = attackPower;
        m_projectileSpeed = projectileSpeed;
        m_projectileRange = projectileRange;
        m_cost = cost;
    }
}