using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerHpViewExtension
{
    public static void RegisterChangeHpEventOnEnable(this PlayerHpViewModel hpvm)
    {
        UIManager.Instance.RegisterChangeHpCallBack(hpvm.OnResponseChangeHp);
    }
    
    public static void UnRegisterChangeHpEventOnDisable(this PlayerHpViewModel hpvm)
    {
        UIManager.Instance.UnRegisterChangeHpCallBack(hpvm.OnResponseChangeHp);
    }

    public static void OnResponseChangeHp(this PlayerHpViewModel hpvm, int Hp)
    {
        hpvm.CurrentHp = Hp;
    }
}
