using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Weapon : MonoBehaviour, IWeapon
{
    protected WeaponData m_weaponData;
    protected float m_weaponAttackPower;
    protected float m_weaponSpeed;

    public abstract void UseWeapon(bool isCharge);
}


