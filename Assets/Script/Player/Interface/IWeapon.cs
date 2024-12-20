using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void SetWeaponData(PlayerWeaponData weaponData, PlayerData playerData);
    public void UseWeapon(bool isCharge);
}
