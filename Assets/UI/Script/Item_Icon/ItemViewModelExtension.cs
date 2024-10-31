public static class ItemViewModelExtension
{
    public static void RegisterChangeItem_EventOnEnable(this ItemViewModel viewModel,
       MVVM soulKeyName, MVVM healthKeyName)
    {
        UIManager.Instance.RegisterUIManager<int>(soulKeyName, viewModel.OnResponseChangeSoul);
        UIManager.Instance.RegisterUIManager<int>(healthKeyName, viewModel.OnResponseChangeHealthItem);
    }

    public static void UnRegisterChangeItem_EventOnDisable(this ItemViewModel viewModel,
        MVVM soulKeyName, MVVM healthKeyName)
    {
        UIManager.Instance.UnRegisterUIManager<int>(soulKeyName, viewModel.OnResponseChangeSoul);
        UIManager.Instance.UnRegisterUIManager<int>(healthKeyName, viewModel.OnResponseChangeHealthItem);
    }

    public static void OnResponseChangeSoul(this ItemViewModel viewModel, int value)
    {
        viewModel.SoulCount = value;
    }

    public static void OnResponseChangeHealthItem(this ItemViewModel viewModel, int value)
    {
        viewModel.HealthCount = value;
    }

}
