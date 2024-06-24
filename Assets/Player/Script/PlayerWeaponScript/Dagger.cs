using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    void Start()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Dagger);
    }

    public override void InitWeapon(GameObject hitRangeObject)
    {

    }

    public override void UseWeapon(bool isCharge, GameObject hitRange)
    {
        
    }
}
