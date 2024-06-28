using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public abstract class Weapon : MonoBehaviour, IWeapon, IWeaponEvent
{
    protected PlayerWeaponEffectController m_weaponEffect;
    protected Action<float, float, Vector3, Vector3> m_action;
    protected Action<bool> m_weaponRangeEvent;
    protected WeaponData m_weaponData;

    public void AddWeaponData(bool isAddEvent, Action<float, float, Vector3, Vector3> callBack)
    {
        if (isAddEvent)
        {
            m_action += callBack;
        }
        else
        {
            m_action -= callBack;
        }
    }

    public void AddUseWeaponEvent(bool isAddEvent, Action<bool> callBack)
    {
        if (isAddEvent)
        {
            m_weaponRangeEvent += callBack;
        }
        else
        {
            m_weaponRangeEvent -= callBack;
        }
    }

    public abstract void UseWeapon(bool isCharge);
}


