using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSkillViewExtension
{
    
    public static void RefreshViewModel(this PlayerSkillViewModel viewModel)
    {
        UIManager.Instance.RefreshSkillInfo(viewModel.OnRefreshViewModel);
    }

    public static void OnRefreshViewModel(this PlayerSkillViewModel viewModel, PlayerSkill skill)
    {
        viewModel.CurrentSkill = skill;
    }

    public static void RegisterChangeSkillEventOnEnable(this PlayerSkillViewModel skillvm)
    {
        UIManager.Instance.RegisterChangeSkillCallBack(skillvm.OnResponseChangeSkill);
    }

    public static void UnRegisterChangeSkillEventOnDisable(this PlayerSkillViewModel skillvm)
    {
        UIManager.Instance.UnRegisterChangeSkillCallBack(skillvm.OnResponseChangeSkill);
    }

    public static void OnResponseChangeSkill(this PlayerSkillViewModel skillvm, PlayerSkill skill)
    {
        skillvm.CurrentSkill = skill;
    }
}
