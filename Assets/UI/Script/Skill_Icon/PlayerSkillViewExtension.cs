public static class PlayerSkillViewExtension
{
    public static void RegisterChangeSkillIcon_EventOnEnable(this PlayerSkillViewModel viewModel,
        MVVM skillKeyName, MVVM skillCountKeyName)
    {
        UIManager.Instance.RegisterUIManager<PlayerSkill>(skillKeyName, viewModel.OnResponseChangeSkill);
        UIManager.Instance.RegisterUIManager<int>(skillCountKeyName, viewModel.OnResponseChangeSkillCount);
    }

    public static void UnRegisterChangeSkillIcon_EventOnDisable(this PlayerSkillViewModel viewModel,
         MVVM skillKeyName, MVVM skillCountKeyName)
    {
        UIManager.Instance.UnRegisterUIManager<PlayerSkill>(skillKeyName, viewModel.OnResponseChangeSkill);
        UIManager.Instance.UnRegisterUIManager<int>(skillCountKeyName, viewModel.OnResponseChangeSkillCount);
    }

    public static void OnResponseChangeSkill(this PlayerSkillViewModel viewModel, PlayerSkill currentSkill)
    {
        viewModel.CurrentSkill = currentSkill;
    }

    public static void OnResponseChangeSkillCount(this PlayerSkillViewModel viewModel, int skillCount)
    {
        viewModel.CurrentSkillCount = skillCount;
    }
}
