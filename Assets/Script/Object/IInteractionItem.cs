using System;
using UnityEngine;
public interface IInteractionItem
{
    public void InteractionItem();
}

public interface IInteractionLadder
{
    public void InteractionLadder(GameObject player);
    public (float,float) LadderLength();
}

public interface ITrinketCameraEvent
{
    public void TrinketCameraEvent(Action<TrinketItemType, bool> callBack, bool isAdd);
}

public interface IWeaponCameraEvent
{
    public void WeaponCameraEvent(Action<PlayerWeapon, bool> callBack, bool isAdd);
}