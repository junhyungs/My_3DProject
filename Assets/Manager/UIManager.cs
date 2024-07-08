using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private PlayerSkill m_currentSkill;
    private PlayerWeapon m_currentWeapon;    

    private Action<PlayerSkill> SkillChangeCallBack;
    private Action<int> PlayerSkillCountCallBack;
    private Action<int> PlayerHpIconChangeCallBack;

    void Start()
    {
        m_currentSkill = SkillManager.Instance.GetCurrentSkill();
    }
















    //HP
    public void RequestChangeHp(int hp)
    {
        PlayerHpIconChangeCallBack?.Invoke(hp);
    }

    public void RegisterChangeHpCallBack(Action<int> hpCallBack)
    {
        PlayerHpIconChangeCallBack += hpCallBack;
    }

    public void UnRegisterChangeHpCallBack(Action<int> hpCallBack)
    {
        PlayerHpIconChangeCallBack -= hpCallBack;
    }

    //Skill
    public void RequestChangeSkill(PlayerSkill changeSkill)
    {
        m_currentSkill = changeSkill;

        SkillChangeCallBack?.Invoke(m_currentSkill);
    }

    public void RequestChangeSkillCount(int skillCount)
    {
        PlayerSkillCountCallBack?.Invoke(skillCount);
    }

    public void RefreshSkillInfo(Action<PlayerSkill> callBack)
    {
        callBack?.Invoke(m_currentSkill);
    }

    public void RegisterChangeSkillCountCallBack(Action<int> skillCountCallBack)
    {
        PlayerSkillCountCallBack += skillCountCallBack;
    }

    public void UnRegisterChangeSkillCountCallBack(Action<int> skillCountCallBack)
    {
        PlayerSkillCountCallBack -= skillCountCallBack;
    }

    public void RegisterChangeSkillCallBack(Action<PlayerSkill> skillChangeCallBack)
    {
        SkillChangeCallBack += skillChangeCallBack;
    }

    public void UnRegisterChangeSkillCallBack(Action<PlayerSkill> skillChangeCallBack)
    {
        SkillChangeCallBack -= skillChangeCallBack;
    }
   
}
