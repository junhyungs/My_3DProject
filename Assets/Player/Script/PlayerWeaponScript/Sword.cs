using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private void OnEnable()
    {
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Sword);
        m_weaponEffect = GetComponent<PlayerWeaponEffectController>();
        m_weaponController = GetComponent<PlayerWeaponController>();
    }

    public override void InitWeapon(GameObject hitRangeObject)
    {
        m_weaponController.SetWeaponRange(m_weaponData.m_normalAttackRange);
        m_weaponEffect.SetEffectRange(m_weaponData.m_normalEffectRange, m_weaponData.m_chargeEffectRange);
    }

    public override void UseWeapon(bool isCharge, GameObject hitRange)
    {
        if (isCharge)
        {
            ChargeAttack(hitRange);
        }
        else
        {
            NormalAttack(hitRange);
        }
    }

    protected override void ChargeAttack(GameObject hitRange)
    {
        m_weaponController.SetWeaponRange(m_weaponData.m_chargeAttackRange);
        m_currentAtk = m_weaponData.m_chargeAttackPower;
    }

    protected override void NormalAttack(GameObject hitRange)
    {
        m_weaponController.SetWeaponRange(m_weaponData.m_normalAttackRange);
        m_currentAtk = m_weaponData.m_attackPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {


        }
    }


}
