using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerWeapon
{
    Sword,
    Hammer,
    Dagger,
    GreatSword,
    Umbrella
}

public class WeaponManager : Singleton<WeaponManager>
{
    private Dictionary<PlayerWeapon, WeaponData> WeaponDataDictionary = new Dictionary<PlayerWeapon, WeaponData>();
    private PlayerWeapon m_currentWeapon;

    private void Awake()
    {
        InitWeaponData();
    }

    private void InitWeaponData()
    {
        WeaponDataDictionary.Add(PlayerWeapon.Sword, new WeaponData(1.0f, 0.4f, 2.5f));
        WeaponDataDictionary.Add(PlayerWeapon.Hammer, new WeaponData(1.15f, 0.5f, 2.5f));
        WeaponDataDictionary.Add(PlayerWeapon.Dagger, new WeaponData(0.8f, 0.35f, 1.8f));
        WeaponDataDictionary.Add(PlayerWeapon.GreatSword, new WeaponData(1.25f, 0.5f, 3.0f));
        WeaponDataDictionary.Add(PlayerWeapon.Umbrella, new WeaponData(0.5f, 0.4f, 2.5f));
    }

    public WeaponData GetWeaponData(PlayerWeapon weapon)
    {
        return WeaponDataDictionary[weapon];
    }

    public void SetCurrentWeapon(PlayerWeapon weapon)
    {
        m_currentWeapon = weapon;
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return m_currentWeapon;
    }
  
}

public struct WeaponData
{
    public float m_attackPower { get; }
    public float m_attackSpeed { get; }
    public float m_attackRange { get; }
    public WeaponData(float attackPower, float attackSpeed, float weaponRange)
    {
        m_attackPower = attackPower;
        m_attackSpeed = attackSpeed;
        m_attackRange = weaponRange;
    }
}

