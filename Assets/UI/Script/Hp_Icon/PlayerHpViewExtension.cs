public static class PlayerHpViewExtension
{
    public static void RegisterChangeHpEventOnEnable(this PlayerHpViewModel hpvm, MVVM keyName)
    {
        UIManager.Instance.RegisterUIManager<int>(keyName, hpvm.OnResponseChangeHp);
    }
    
    public static void UnRegisterChangeHpEventOnDisable(this PlayerHpViewModel hpvm, MVVM keyName)
    {
        UIManager.Instance.UnRegisterUIManager<int>(keyName, hpvm.OnResponseChangeHp);
    }

    public static void OnResponseChangeHp(this PlayerHpViewModel hpvm, int Hp)
    {
        hpvm.CurrentHp = Hp;
    }
}
