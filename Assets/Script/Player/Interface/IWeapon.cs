using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void SetWeaponData(PlayerWeaponData weaponData);
    public void UseWeapon(bool isCharge);

}
