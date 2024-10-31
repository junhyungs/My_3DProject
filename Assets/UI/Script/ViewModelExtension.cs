using System;

public static class ViewModelExtension
{
    private static void RegisterUIMamager<T>(MVVM key, Action<T> callBack)
    {
        UIManager.Instance.RegisterUIManager(key, callBack);
    }

    private static void UnRegisterUIManager<T>(MVVM key, Action<T> callBack)
    {
        UIManager.Instance.UnRegisterUIManager(key, callBack);
    }

    #region Health_Event
    public static void RegisterChangeHealth_EventOnEnable(this ViewModel viewModel, MVVM keyName)
    {
        RegisterUIMamager<int>(keyName, viewModel.OnResponseChageHP);
    }

    public static void UnRegisterChangeHealth_EventOnDisable(this ViewModel viewModel, MVVM keyName)
    {
        UnRegisterUIManager<int>(keyName, viewModel.OnResponseChageHP);
    }

    public static void OnResponseChageHP(this ViewModel viewModel, int hp)
    {
        viewModel.CurrentHP = hp;
    }
    #endregion

    #region Item_Event
    public static void RegisterChangeItem_EventOnEnable(this ViewModel viewModel,
        MVVM soulKeyName, MVVM healthKeyName)
    {
        RegisterUIMamager<int>(soulKeyName, viewModel.OnResponseChangeSoul);
        RegisterUIMamager<int>(healthKeyName, viewModel.OnResponseChangeHealthItem);
    }

    public static void UnRegisterChangeItem_EventOnDisable(this ViewModel viewModel,
        MVVM soulKeyName, MVVM healthKeyName)
    {
        UnRegisterUIManager<int>(soulKeyName, viewModel.OnResponseChangeSoul);
        UnRegisterUIManager<int>(healthKeyName, viewModel.OnResponseChangeHealthItem);
    }

    public static void OnResponseChangeSoul(this ViewModel viewModel, int value)
    {
        viewModel.SoulCount = value;
    }

    public static void OnResponseChangeHealthItem(this ViewModel viewModel, int value)
    {
        viewModel.HealthItemCount = value;
    }
    #endregion

    #region SkillIcon_Event
    public static void RegisterChangeSkillIcon_EventOnEnable(this ViewModel viewModel,
        MVVM skillKeyName, MVVM skillCountKeyName)
    {
        RegisterUIMamager<PlayerSkill>(skillKeyName, viewModel.OnResponseChangeSkill);
        RegisterUIMamager<int>(skillCountKeyName, viewModel.OnResponseChangeSkillCount);
    }

    public static void UnRegisterChangeSkillIcon_EventOnDisable(this ViewModel viewModel,
         MVVM skillKeyName, MVVM skillCountKeyName)
    {
        UnRegisterUIManager<PlayerSkill>(skillKeyName, viewModel.OnResponseChangeSkill);
        UnRegisterUIManager<int>(skillCountKeyName, viewModel.OnResponseChangeSkillCount);
    }

    public static void OnResponseChangeSkill(this ViewModel viewModel, PlayerSkill currentSkill)
    {
        viewModel.CurrentSkill = currentSkill;
    }

    public static void OnResponseChangeSkillCount(this ViewModel viewModel, int skillCount)
    {
        viewModel.CurrentSkillCount = skillCount;
    }
    #endregion
}
