using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon // �ǰ� ���� �ݶ��̴��� ���� ������ ���ݷ�, ������ �����ϴ� �������̽� �ۼ��ؼ� ���
{
    public Sword(string weaponName, float normalPower, float chargePower, float normalEffectRange, float chargeEffectRange, Vector3 normalAttackRange, Vector3 chargeAttackRange) : base(weaponName, normalPower, chargePower, normalEffectRange, chargeEffectRange, normalAttackRange, chargeAttackRange)
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
        m_weaponEffect = GetComponent<PlayerWeaponEffectController>();
        m_weaponController = GetComponent<PlayerWeaponController>();
    }

    public override void InitWeapon()
    {
        m_weaponController.SetWeaponRange(m_normalAttackRange);
        m_weaponEffect.SetEffectRange(m_normalEffect, m_chargeEffect);
    }

    public override void UseWeapon(bool isCharge)
    {
        if (isCharge)
        {
            ChargeAttack();
        }
        else
        {
            NormalAttack();
        }
    }

    protected override void ChargeAttack()
    {
        m_weaponController.SetWeaponRange(m_chargeAttackRange);
    }

    protected override void NormalAttack()
    {
        m_weaponController.SetWeaponRange(m_normalAttackRange);
    }
  
}
