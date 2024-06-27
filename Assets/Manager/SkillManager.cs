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
        set { m_skillCount = value; }
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SkillDictionary.Add(PlayerSkill.Bow, new SkillData(1, 500.0f, 0f, 1));
        SkillDictionary.Add(PlayerSkill.FireBall, new SkillData(1, 200.0f, 1f, 1));
        SkillDictionary.Add(PlayerSkill.Bomb, new SkillData(1, 10.0f, 10.0f, 2));
        SkillDictionary.Add(PlayerSkill.Hook, new SkillData(1, 10.0f, 5.0f, 0));

        m_skillCount = 4;
    }

    public SkillData GetSkillData(PlayerSkill skill)
    {
        return SkillDictionary[skill];
    }

    public void SetCurretSkill(PlayerSkill skill)
    {
        m_currentSkill = skill;
    }

    public void AddSkill(PlayerSkill skill)
    {
        if (HasSkill(skill))
        {
            Debug.Log("스킬 등록함");
            ObtainedSkill.Add(skill);
        }
    }

    public bool HasSkill(PlayerSkill skill)
    {
        if (!ObtainedSkill.Contains(skill))
        {
            Debug.Log("스킬이 HashSet에 없음");
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
    public int m_attackPower { get; }
    public float m_projectileSpeed { get; }
    public float m_projectileRange { get; }
    public int m_cost { get; }
    public SkillData(int attackPower,float projectileSpeed, float projectileRange, int cost)
    {
        m_attackPower = attackPower;
        m_projectileSpeed = projectileSpeed;
        m_projectileRange = projectileRange;
        m_cost = cost;
    }
}