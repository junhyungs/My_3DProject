using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSkillViewExtension
{

    public static void RegisterChangeSkillEventOnEnable(this PlayerSkillViewModel skillvm)
    {
        UIManager.Instance.RegisterChangeSkillCallBack(skillvm.OnResponseChangeSkill);
        UIManager.Instance.RegisterChangeSkillCountCallBack(skillvm.OnResponseChangeSkillCount);
    }

    public static void UnRegisterChangeSkillEventOnDisable(this PlayerSkillViewModel skillvm)
    {
        UIManager.Instance.UnRegisterChangeSkillCallBack(skillvm.OnResponseChangeSkill);
        UIManager.Instance.UnRegisterChangeSkillCountCallBack(skillvm.OnResponseChangeSkillCount);
    }

    public static void OnResponseChangeSkill(this PlayerSkillViewModel skillvm, PlayerSkill skill)
    {
        skillvm.CurrentSkill = skill;
    }

    public static void OnResponseChangeSkillCount(this  PlayerSkillViewModel skillvm, int skillCount)
    {
        skillvm.CurrentSkillCount = skillCount;
    }
}
