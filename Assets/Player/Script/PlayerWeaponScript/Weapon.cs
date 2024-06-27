using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public abstract class Weapon : MonoBehaviour, IWeapon
{
    public Weapon(string weaponName, float normalPower, float chargePower, float normalEffectRange,
        float chargeEffectRange, Vector3 normalAttackRange, Vector3 chargeAttackRange)
    {
        m_weaponName = weaponName;
        m_normalPower = normalPower;
        m_chargePower = chargePower;
        m_normalEffect = normalEffectRange;
        m_chargeEffect = chargeEffectRange;
        m_normalAttackRange = normalAttackRange;
        m_chargeAttackRange = chargeAttackRange;
    }

    protected PlayerWeaponEffectController m_weaponEffect;
    protected PlayerWeaponController m_weaponController;
    protected string m_weaponName;
    protected float m_normalPower;
    protected float m_chargePower;
    protected float m_normalEffect;
    protected float m_chargeEffect;
    protected Vector3 m_normalAttackRange;
    protected Vector3 m_chargeAttackRange;

    public abstract void InitWeapon();
    public abstract void UseWeapon(bool isCharge);
    protected virtual void ChargeAttack() { }
    protected virtual void NormalAttack() { }
  
}


