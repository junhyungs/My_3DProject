using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public abstract class Weapon : MonoBehaviour, IWeapon
{
    protected PlayerWeaponEffectController m_weaponEffect;
    protected PlayerWeaponController m_weaponController;
    protected WeaponData m_weaponData;
    protected float m_currentAtk;

    public abstract void InitWeapon();
    public abstract void UseWeapon(bool isCharge);
    protected virtual void ChargeAttack() { }
    protected virtual void NormalAttack() { }
  
}


