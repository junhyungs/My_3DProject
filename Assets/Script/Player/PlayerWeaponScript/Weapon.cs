using System;
using UnityEngine;



public abstract class Weapon : MonoBehaviour, IWeapon
{
    protected PlayerWeaponEffectController m_weaponEffect;
    protected PlayerWeaponData _weaponData;

    protected Vector3 _forward;
    protected Vector3 _boxPosition;

    protected LayerMask _targetLayer;
    protected float _currentPower;

    public abstract void SetWeaponData(PlayerWeaponData weaponData);
    public abstract void UseWeapon(bool isCharge);
}


