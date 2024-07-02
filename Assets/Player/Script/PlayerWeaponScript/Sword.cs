using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class Sword : Weapon
{
    private void OnEnable()
    {
        EventManager.Instance.RegisterSetWeaponDataEvent(this);
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Sword);
        m_weaponEffect = GetComponent<PlayerWeaponEffectController>();
        m_weaponEffect.SetEffectRange(m_weaponData.m_defaultEffectRange, m_weaponData.m_chargeEffectRange);
    }

    public override void InitAttackObject()
    {
        m_weaponRangeEvent.Invoke(m_weaponData.m_defaultPower, m_weaponData.m_chargePower,
        m_weaponData.m_defaultAttackRange, m_weaponData.m_chargeAttackRange);
    }
}
