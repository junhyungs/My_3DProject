using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    private void Start()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Bow);
    }

    public override void UseWeapon()
    {
        Debug.Log("활 사용함");
    }
}
