using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerWeapon
{
    Sword,
    Bow
}

public class WeaponManager : Singleton<WeaponManager>
{
    private Dictionary<PlayerWeapon, WeaponData> WeaponDataDictionary = new Dictionary<PlayerWeapon, WeaponData>();
    private Dictionary<PlayerWeapon, Weapon> WeaponDictionary = new Dictionary<PlayerWeapon, Weapon>();
    private Dictionary<PlayerWeapon, GameObject> WeaponPrefabDictionary = new Dictionary<PlayerWeapon, GameObject>();

    private void Awake()
    {
        InitWeapon();
    }

    private void InitWeapon()
    {
        WeaponDataDictionary.Add(PlayerWeapon.Sword, new WeaponData(15.0f, 5.0f, false));
        WeaponDataDictionary.Add(PlayerWeapon.Bow, new WeaponData(15.0f, 10.0f, true));
    }

    public void AddWeapon(PlayerWeapon weapon, Weapon addWeapon)
    {
        WeaponDictionary.Add(weapon, addWeapon);
    }

    public void AddWeaponPrefab(PlayerWeapon weapon, GameObject weaponPrefab)
    {
        WeaponPrefabDictionary.Add(weapon, weaponPrefab);
    }

    public Weapon GetWeapon(PlayerWeapon weapon)
    {
        return WeaponDictionary[weapon];
    }

    public GameObject GetWeaponPrefab(PlayerWeapon weapon)
    {
        return WeaponPrefabDictionary[weapon];
    }

    public WeaponData GetWeaponData(PlayerWeapon weapon)
    {
        return WeaponDataDictionary[weapon];
    }
}

public struct WeaponData
{
    //weaponRange : true = long, false = short
    public bool weaponRange { get; }
    public float m_attackPower { get; }
    public float m_attackSpeed { get; }

    public WeaponData(float attackPower, float attackSpeed, bool weaponRange)
    {
        m_attackPower = attackPower;
        m_attackSpeed = attackSpeed;
        this.weaponRange = weaponRange;
    }

}

