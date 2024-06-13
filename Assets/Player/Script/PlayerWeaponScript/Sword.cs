using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private void Start()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Sword);
    }

    public override void UseWeapon()
    {
        
    }
}
