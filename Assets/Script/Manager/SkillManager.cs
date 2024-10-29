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
    private Dictionary<PlayerSkill, Skill> _skillDictionary = new Dictionary<PlayerSkill, Skill>();
    private int m_skillCount;

    private void Start()
    {
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

            UIManager.Instance.RequestChangeSkillCount(m_skillCount);
        }
    }

    public bool Cost(PlayerSkill skillName)
    {
        if(m_skillCount <= 0)
        {
            return false;
        }

        Skill skillComponent = _skillDictionary[skillName];

        if(skillComponent != null)
        {
            var cost = skillComponent.GetSkillData().Cost;

            var calculate = m_skillCount - cost;

            if(calculate >= 0)
            {
                m_skillCount -= cost;

                UIManager.Instance.RequestChangeSkillCount(m_skillCount);

                return true;
            }
        }

        return false;
    }

    private IEnumerator CreateSkill()
    {
     
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData("Bow") == null;
        });

        Debug.Log("스킬 데이터를 가져왔습니다.");

        Array enumArray = Enum.GetValues(typeof(PlayerSkill));

        for(int i = 0; i < enumArray.Length - 1; i++)
        {
            string id = enumArray.GetValue(i).ToString();

            var data = DataManager.Instance.GetData(id) as PlayerSkillData;

            switch ((PlayerSkill)enumArray.GetValue(i))
            {
                case PlayerSkill.Bow:
                    Bow bow = new Bow();
                    bow.SetSkillData(data);
                    _skillDictionary.Add(PlayerSkill.Bow, bow);
                    break;
                case PlayerSkill.FireBall:
                    FireBall fireBall = new FireBall();
                    fireBall.SetSkillData(data);
                    _skillDictionary.Add(PlayerSkill.FireBall, fireBall);
                    break;
                case PlayerSkill.Bomb:
                    Bomb bomb = new Bomb();
                    bomb.SetSkillData(data);
                    _skillDictionary.Add(PlayerSkill.Bomb, bomb);
                    break;
                case PlayerSkill.Hook:
                    Hook hook = new Hook();
                    hook.SetSkillData(data);
                    _skillDictionary.Add(PlayerSkill.Hook, hook);
                    break;
            }
        }
    }
 
    public Skill GetSkill(PlayerSkill skill)
    {
        if (_skillDictionary.ContainsKey(skill))
        {
            return _skillDictionary[skill];
        }

        return null;
    }
}