using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public abstract class Weapon : MonoBehaviour, IWeapon
{
    protected WeaponData m_weaponData;
    protected PlayerWeaponEffectController m_weaponEffect;
    protected float m_currentAtk;

    public abstract void InitWeapon(GameObject hitRangeObject);

    public abstract void UseWeapon(bool isCharge, GameObject hitRange);

    protected virtual void ChargeAttack(GameObject hitRangeObject) { }
    protected virtual void NormalAttack(GameObject hitRangeObject) { }
  
}


