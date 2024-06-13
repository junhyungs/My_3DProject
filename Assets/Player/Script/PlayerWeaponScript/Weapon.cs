using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weapon : MonoBehaviour, IWeapon
{
    protected WeaponData m_weaponData;

    public virtual void UseWeapon() { }
}


