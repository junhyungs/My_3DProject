using System;
using System.Collections;
using System.Collections.Generic;
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
    private Dictionary<PlayerSkill, ISkill> _skillDictionary = new Dictionary<PlayerSkill, ISkill>();
    private int m_skillCount;

    private void Start()
    {
        CreateSkillProjectile();
        StartCoroutine(CreateSkill());
    }

    public int SkillCount
    {
        get { return m_skillCount; }
        set
        {
            if (value > 4)
            {
                return;
            }

            m_skillCount = value;

            UIManager.Instance.TriggerEvent(MVVM.SkillCount_Event, m_skillCount);
        }
    }

    public bool Cost(PlayerSkill skillName)
    {
        if(m_skillCount <= 0 && !(skillName is PlayerSkill.Hook))
        {
            return false;
        }

        ISkill skillComponent = _skillDictionary[skillName];

        if(skillComponent != null)
        {
            var cost = skillComponent.GetSkillData().Cost;

            var calculate = m_skillCount - cost;

            if(calculate >= 0)
            {
                m_skillCount -= cost;

                UIManager.Instance.TriggerEvent(MVVM.SkillCount_Event, m_skillCount);

                return true;
            }
        }

        return false;
    }

    private void CreateSkillProjectile()
    {
        var projectileValue = new (ObjectName, int)[]
        {
            (ObjectName.PlayerHook, 1),
            (ObjectName.PlayerSegment, 20),
            (ObjectName.PlayerArrow, 5),
            (ObjectName.PlayerBomb, 3),
            (ObjectName.PlayerFireBall, 5),
            (ObjectName.HitEffect, 5)
        };

        foreach(var(projectileType, count) in projectileValue)
        {
            ObjectPool.Instance.CreatePool(projectileType, count);
        }
    }

    private IEnumerator CreateSkill()
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData("Bow") == null;
        });

        Array enumArray = Enum.GetValues(typeof(PlayerSkill));

        for(int i = 0; i < enumArray.Length - 1; i++)
        {
            string id = enumArray.GetValue(i).ToString();

            var data = DataManager.Instance.GetData(id) as PlayerSkillData;

            ISkill skill = null;

            var enumValue = (PlayerSkill)enumArray.GetValue(i);

            switch (enumValue)
            {
                case PlayerSkill.Bow:
                    skill = new Bow();
                    break;
                case PlayerSkill.FireBall:
                    skill = new FireBall();
                    break;
                case PlayerSkill.Bomb:
                    skill = new Bomb();
                    break;
                case PlayerSkill.Hook:
                    skill = new Hook();
                    break;
            }

            skill.SetSkillData(data);

            _skillDictionary.Add(enumValue, skill);
        }
    }
 
    public ISkill GetSkill(PlayerSkill skill)
    {
        if (_skillDictionary.ContainsKey(skill))
        {
            return _skillDictionary[skill];
        }

        return null;
    }
}