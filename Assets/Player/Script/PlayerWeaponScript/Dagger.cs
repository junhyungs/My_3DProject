using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    void Start()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Dagger);
    }

    public override void InitWeapon(Vfx_Controller effectRange, GameObject hitRangeObject)
    {

    }

    public override void UseWeapon(bool isCharge, Vfx_Controller effectRange, GameObject hitRange)
    {
        
    }
}
