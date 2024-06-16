using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{
    private void Start()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.GreatSword);
    }
    public override void UseWeapon()
    {
        
    }
}
