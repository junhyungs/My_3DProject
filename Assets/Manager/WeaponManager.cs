using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerWeapon
{
    Sword,
    Bow,
    FireBall,

}

public class WeaponManager : Singleton<WeaponManager>
{
    [Header("PlayerUseWeapon")]
    [SerializeField] private GameObject[] m_playerUseWeapon_Prefab;
    [Header("PlayerIdleWeapon")]
    [SerializeField] private GameObject[] m_playerIdleWeapon_Prefab;

    private IWeapon m_weapon;
    private PlayerWeapon m_currentWeapon;

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

        m_currentWeapon = PlayerWeapon.Sword;
        AllDisableWeapon();
    }

    

   

    public void StopWeapon()
    {
        if(m_currentWeapon == PlayerWeapon.Bow)
        {
            m_playerUseWeapon_Prefab[(int)m_currentWeapon].SetActive(false);
            return;
        }

        ActiveWeapon(m_currentWeapon, false);
    }

    public void ActiveWeapon(PlayerWeapon weapon, bool isUsing)
    {
        if (isUsing)
        {
            if (m_playerUseWeapon_Prefab[(int)weapon] != null)
            {
                m_playerUseWeapon_Prefab[(int)weapon].SetActive(true);
            }
        }
        else
        {
            if (m_playerIdleWeapon_Prefab[(int)weapon] != null)
            {
                m_playerIdleWeapon_Prefab[(int)weapon].SetActive(true);
            }
        }
    }


    

    public void AddWeapon(PlayerWeapon weapon, Weapon addWeapon)
    {
        WeaponDictionary.Add(weapon, addWeapon);
    }

    public bool ContainsWeapon(PlayerWeapon weapon)
    {
        if(WeaponDictionary.ContainsKey(weapon))
        {
            return true;
        }
        else
        {
            return false;
        }
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

    private void AllDisableWeapon()
    {
        foreach(var weapon in m_playerUseWeapon_Prefab)
        {
            if(weapon != null)
            {
                weapon.SetActive(false);
            }
        }

        foreach(var weapon in m_playerIdleWeapon_Prefab)
        {
            if(weapon != null)
            {
                weapon.SetActive(false);
            }
        }
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

