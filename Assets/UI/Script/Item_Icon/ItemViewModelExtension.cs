using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemViewModelExtension
{
    public static void RegisterChangeValueEventOnEnable(this ItemViewModel itemVm)
    {
        InventoryManager.Instance.RegisterChangeSoulValueCallBack(itemVm.OnResponseChangeSoul);
        InventoryManager.Instance.RegisterChangeHealthValueCallBack(itemVm.OnResponeseChangeHealth);
    }

    public static void UnRegisterChangeValueEventOnDisable(this ItemViewModel itemVm)
    {
        InventoryManager.Instance.UnRegisterChangeSoulValueCallBack(itemVm.OnResponseChangeSoul);
        InventoryManager.Instance.UnRegisterChangeHealthValueCallBack(itemVm.OnResponeseChangeHealth);
    }

    public static void OnResponseChangeSoul(this ItemViewModel itemVm, int value)
    {
        itemVm.SoulCount = value;
    }

    public static void OnResponeseChangeHealth(this ItemViewModel itemVm, int value)
    {
        itemVm.HealthCount = value;
    }

}
