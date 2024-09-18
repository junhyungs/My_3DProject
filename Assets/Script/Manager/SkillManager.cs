using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public enum PlayerSkill
{
    Bow,
    FireBall,
    Bomb,
    Hook,

    None
}

public class SkillManager : Singleton<SkillManager>
{
    private HashSet<PlayerSkill> ObtainedSkill = new HashSet<PlayerSkill>();
    private Dictionary<PlayerSkill, Skill> _skillDictionary = new Dictionary<PlayerSkill, Skill>();
    private PlayerSkill m_currentSkill;

    private int m_skillCount;

    private void Start()
    {
        CreateSkillObject();
    }

    private void CreateSkillObject()
    {
        ObjectPool.Instance.CreatePool(ObjectName.PlayerHook, 1);
        ObjectPool.Instance.CreatePool(ObjectName.PlayerSegment);
        ObjectPool.Instance.CreatePool(ObjectName.PlayerArrow);
        ObjectPool.Instance.CreatePool(ObjectName.PlayerBomb);
        ObjectPool.Instance.CreatePool(ObjectName.PlayerFireBall);
        ObjectPool.Instance.CreatePool(ObjectName.HitEffect);
    }

    public int SkillCount
    {
        get { return m_skillCount; }
        set
        {
            if (m_skillCount >= 4)
            {
                return;
            }

            m_skillCount = value;
            UIManager.Instance.RequestChangeSkillCount(m_skillCount);
        }
    }

    public void Cost(PlayerSkill skill)
    {
        if(m_skillCount <= 0)
        {
            return;
        }

        Skill currentSkill = _skillDictionary[skill];

        int cost = currentSkill.GetSkillData().Cost;

        m_skillCount -= cost;

        UIManager.Instance.RequestChangeSkillCount(m_skillCount);
    }

    public IEnumerator LoadSkillData(string id, Skill skillComponent)
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("스킬 데이터를 가져오지 못했습니다.");
            return DataManager.Instance.GetData(id) == null;
        });

        var skillData = DataManager.Instance.GetData(id) as PlayerSkillData;

        skillComponent.SetSkillData(skillData);
    }

    public void SetCurretSkill(PlayerSkill skill)
    {
        m_currentSkill = skill;
    }

    public void AddSkill(PlayerSkill skill)
    {
        if (HasSkill(skill))
        {
            GetSkillData(skill);
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

    private void GetSkillData(PlayerSkill skill)
    {
        switch(skill)
        {
            case PlayerSkill.Bow:
                Skill bow = new Bow();
                StartCoroutine(LoadSkillData("S101", bow));
                _skillDictionary.Add(skill, bow);
                break;
            case PlayerSkill.FireBall:
                Skill fireBall = new FireBall();
                StartCoroutine(LoadSkillData("S102", fireBall));
                _skillDictionary.Add(skill, fireBall);
                break;
            case PlayerSkill.Bomb:
                Skill bomb = new Bomb();
                StartCoroutine(LoadSkillData("S103", bomb));
                _skillDictionary.Add(skill, bomb);
                break;
            case PlayerSkill.Hook:
                Skill hook = new Hook();
                StartCoroutine(LoadSkillData("S104", hook));
                _skillDictionary.Add(skill, hook);
                break;
        }
    }

    public Skill GetSkill(PlayerSkill skill)
    {
        return _skillDictionary[skill];
    }
}