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

    void Start()
    {
        m_currentSkill = SkillManager.Instance.GetCurrentSkill();
    }

















    public void RequestChangeSkill(PlayerSkill changeSkill)
    {
        m_currentSkill = changeSkill;

        Debug.Log(m_currentSkill);
        SkillChangeCallBack.Invoke(m_currentSkill);
    }

    public void RefreshSkillInfo(Action<PlayerSkill> callBack)
    {
        callBack?.Invoke(m_currentSkill);
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
