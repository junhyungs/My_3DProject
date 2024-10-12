using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionItem
{
    public void InteractionItem();
}

public interface ITrinketCameraEvent
{
    public void TrinketCameraEvent(Action<TrinketItemType, bool> callBack, bool isAdd);
}

public interface IWeaponCameraEvent
{
    public void WeaponCameraEvent(Action<PlayerWeapon, bool> callBack, bool isAdd);
}