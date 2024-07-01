using System;
using UnityEngine;



public abstract class Weapon : MonoBehaviour, IWeapon, IWeaponEvent
{
    protected PlayerWeaponEffectController m_weaponEffect;
    protected Action<float, float, Vector3, Vector3> m_weaponRangeEvent;
    protected WeaponData m_weaponData;

    public void AddWeaponDataEvent(bool isAddEvent, Action<float, float, Vector3, Vector3> callBack)
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

    public abstract void InitAttackObject();
}


