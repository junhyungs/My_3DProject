using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{
    public GreatSword(string weaponName, float normalPower, float chargePower, float normalEffectRange, float chargeEffectRange, Vector3 normalAttackRange, Vector3 chargeAttackRange) : base(weaponName, normalPower, chargePower, normalEffectRange, chargeEffectRange, normalAttackRange, chargeAttackRange)
    {
        m_weaponName = weaponName;
        m_normalPower = normalPower;
        m_chargePower = chargePower;
        m_normalEffect = normalEffectRange;
        m_chargeEffect = chargeEffectRange;
        m_normalAttackRange = normalAttackRange;
        m_chargeAttackRange = chargeAttackRange;
    }

    private void OnEnable()
    {
        
    }

    public override void InitWeapon()
    {

    }

    public override void UseWeapon(bool isCharge)
    {
        
    }

}
