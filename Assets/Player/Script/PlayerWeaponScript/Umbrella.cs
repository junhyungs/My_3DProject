using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : Weapon
{
    private void Start()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Umbrella);
    }
    public override void UseWeapon()
    {
        
    }
}
