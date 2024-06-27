using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    public Dagger(string weaponName, float normalPower, float chargePower, float normalEffectRange, float chargeEffectRange, Vector3 normalAttackRange, Vector3 chargeAttackRange) : base(weaponName, normalPower, chargePower, normalEffectRange, chargeEffectRange, normalAttackRange, chargeAttackRange)
    { 



    }

 

    public override void InitWeapon()
    {

    }

    public override void UseWeapon(bool isCharge)
    {
        
    }
}
