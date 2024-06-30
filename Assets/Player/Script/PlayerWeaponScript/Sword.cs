using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class Sword : Weapon
{
    private void OnEnable()
    {
        WeaponManager.Instance.RegisterWeaponEvent(this);
        m_weaponData = WeaponManager.Instance.GetWeaponData(PlayerWeapon.Sword);
        m_weaponEffect = GetComponent<PlayerWeaponEffectController>();
        m_weaponEffect.SetEffectRange(m_weaponData.m_defaultEffectRange, m_weaponData.m_chargeEffectRange);
    }

    private void Start()
    {
        m_action?.Invoke(m_weaponData.m_defaultPower, m_weaponData.m_chargePower,
            m_weaponData.m_defaultAttackRange, m_weaponData.m_chargeAttackRange);
    }

    private void OnDisable()
    {
        m_action = null;
    }

    public override void UseWeapon(bool isCharge)
    {
        m_weaponRangeEvent?.Invoke(isCharge);
    }

}
