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
    }

    public void AddSkill(PlayerSkill skillName, Skill skill)
    {
        GetSkillDictionary.Add(skillName, skill);
    }

    public SkillData GetSkillData(PlayerSkill skill)
    {
        return SkillDictionary[skill];
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