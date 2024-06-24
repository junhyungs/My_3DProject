using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private void Start()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Sword);
        m_weaponAttackPower = m_weaponData.m_attackPower;
        m_chargeAttackPower = m_weaponData.m_chargeAttackPower;
        m_normalEffectRange = m_weaponData.m_normalEffectRange;
        m_chargeEffectRange = m_weaponData.m_chargeEffectRange;
        m_normalAttackRange = m_weaponData.m_normalAttackRange;
        m_chargeAttackRange = m_weaponData.m_chargeAttackRange;
    }

    public override void InitWeapon(Vfx_Controller effectRange, GameObject hitRangeObject)
    {
        effectRange.NormalAttackSize = m_weaponData.m_normalEffectRange;
        effectRange.ChargeAttackSize = m_weaponData.m_chargeEffectRange;
        Debug.Log(effectRange.NormalAttackSize);
        Debug.Log(effectRange.ChargeAttackSize);

        hitRangeObject.transform.localScale = m_normalAttackRange;
    }

    public override void UseWeapon(bool isCharge, Vfx_Controller effectRange, GameObject hitRange)
    {
        effectRange.IsCharge(isCharge);
        hitRange.transform.localScale = isCharge ? m_chargeAttackRange : m_normalAttackRange;
        Debug.Log(hitRange);
        m_currentAttackPower = isCharge ? m_chargeAttackPower : m_weaponAttackPower;
        Debug.Log(m_currentAttackPower);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {


        }
    }


}
