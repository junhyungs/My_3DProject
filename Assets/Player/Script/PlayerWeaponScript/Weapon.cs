using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public abstract class Weapon : MonoBehaviour, IWeapon
{
    protected WeaponData m_weaponData;
    protected float m_weaponAttackPower;
    protected float m_chargeAttackPower;
    protected float m_normalEffectRange;
    protected float m_chargeEffectRange;
    protected float m_currentAttackPower;
    protected Vector3 m_normalAttackRange;
    protected Vector3 m_chargeAttackRange;

    public abstract void InitWeapon(Vfx_Controller effectRange, GameObject hitRangeObject);

    public abstract void UseWeapon(bool isCharge, Vfx_Controller effectRange, GameObject hitRange);
  
}


