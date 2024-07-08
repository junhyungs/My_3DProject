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
    private Dictionary<PlayerSkill, SkillData> SkillDictionary = new Dictionary<PlayerSkill, SkillData>();
    private HashSet<PlayerSkill> ObtainedSkill = new HashSet<PlayerSkill>();
    private PlayerSkill m_currentSkill;
    private int m_skillCount;

    public int SkillCount
    {
        get { return m_skillCount; }
        set
        {
            m_skillCount = value;
            UIManager.Instance.RequestChangeSkillCount(m_skillCount);
        }
    }

    private void Awake()
    {
        InitializeSkillData();
    }

    private void InitializeSkillData()
    {
        InitSkill(PlayerSkill.Bow, "Bow");
        InitSkill(PlayerSkill.FireBall, "FireBall");
        InitSkill(PlayerSkill.Bomb, "Bomb");
        InitSkill(PlayerSkill.Hook, "Hook");

        m_skillCount = 4;

        UIManager.Instance.RequestChangeSkillCount(m_skillCount);
    }

    private void InitSkill(PlayerSkill skill, string skillName)
    {
        var skillData = DataManager.Instance.GetSkillData(skillName);
        SkillData data = new SkillData(
            skillData.SkillName,
            skillData.SkillAttackPower,
            skillData.ProjectileSpeed,
            skillData.SkillRange,
            skillData.Cost);

        SkillDictionary.Add(skill, data);
    }

    public SkillData GetSkillData(PlayerSkill skill)
    {
        return SkillDictionary[skill];
    }

    public void SetCurretSkill(PlayerSkill skill)
    {
        m_currentSkill = skill;
    }

    public PlayerSkill GetCurrentSkill()
    {
        return m_currentSkill;
    }

    public void AddSkillCount()
    {
        if (m_skillCount >= 4)
            return;

        m_skillCount++;
        UIManager.Instance.RequestChangeSkillCount(m_skillCount);
    }

    public void AddSkill(PlayerSkill skill)
    {
        if (HasSkill(skill))
        {
            ObtainedSkill.Add(skill);   
        }
    }

    public bool HasSkill(PlayerSkill skill)
    {
        if (!ObtainedSkill.Contains(skill))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public struct SkillData
{
    public string m_skillName { get; }
    public float m_attackPower { get; }
    public float m_projectileSpeed { get; }
    public float m_projectileRange { get; }
    public int m_cost { get; }
    public SkillData(string skillName ,float attackPower,float projectileSpeed, float projectileRange, int cost)
    {
        m_skillName = skillName;
        m_attackPower = attackPower;
        m_projectileSpeed = projectileSpeed;
        m_projectileRange = projectileRange;
        m_cost = cost;
    }
}