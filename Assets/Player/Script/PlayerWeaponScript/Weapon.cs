using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Weapon : MonoBehaviour, IWeapon
{
    protected WeaponData m_weaponData;
    public abstract void UseWeapon();
}


