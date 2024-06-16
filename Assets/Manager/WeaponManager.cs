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
    [Header("PlayerUseWeapon")]
    [SerializeField] private GameObject[] m_playerUseWeapon_Prefab;

    [Header("PlayerIdleWeapon")]
    [SerializeField] private GameObject[] m_playerIdleWeapon_Prefab;

    [Header("PlayerPrefab")]
    [SerializeField] private GameObject m_player;

    private IWeapon m_currentWeapon;
    private PlayerWeapon m_weaponType;
    private Dictionary<PlayerWeapon, WeaponData> WeaponDataDictionary = new Dictionary<PlayerWeapon, WeaponData>();    
    private Dictionary<PlayerWeapon, GameObject> UseWeaponObject = new Dictionary<PlayerWeapon, GameObject>();
    private Dictionary<PlayerWeapon, GameObject> IdleWeaponObject = new Dictionary<PlayerWeapon, GameObject>();

    private void Awake()
    {
        InitWeaponData();
        InitWeaponObject();
        InitIdleWeaponObject();
        AllDisableWeapon();

        SetWeapon(PlayerWeapon.Sword);
    }

    private void InitWeaponData()
    {
        WeaponDataDictionary.Add(PlayerWeapon.Sword, new WeaponData(1.0f, 0.4f, 2.5f));
        WeaponDataDictionary.Add(PlayerWeapon.Hammer, new WeaponData(1.15f, 0.5f, 2.5f));
        WeaponDataDictionary.Add(PlayerWeapon.Dagger, new WeaponData(0.8f, 0.35f, 1.8f));
        WeaponDataDictionary.Add(PlayerWeapon.GreatSword, new WeaponData(1.25f, 0.5f, 3.0f));
        WeaponDataDictionary.Add(PlayerWeapon.Umbrella, new WeaponData(0.5f, 0.4f, 2.5f));
    }

    private void InitWeaponObject()
    {
        UseWeaponObject.Add(PlayerWeapon.Sword, m_playerUseWeapon_Prefab[0]);
        UseWeaponObject.Add(PlayerWeapon.Hammer, m_playerUseWeapon_Prefab[1]);
        UseWeaponObject.Add(PlayerWeapon.Dagger, m_playerUseWeapon_Prefab[2]);
        UseWeaponObject.Add(PlayerWeapon.GreatSword, m_playerUseWeapon_Prefab[3]);
        UseWeaponObject.Add(PlayerWeapon.Umbrella, m_playerUseWeapon_Prefab[4]);
    }

    private void InitIdleWeaponObject()
    {
        IdleWeaponObject.Add(PlayerWeapon.Sword, m_playerIdleWeapon_Prefab[0]);
        IdleWeaponObject.Add(PlayerWeapon.Hammer, m_playerIdleWeapon_Prefab[1]);
        IdleWeaponObject.Add(PlayerWeapon.Dagger, m_playerIdleWeapon_Prefab[2]);
        IdleWeaponObject.Add(PlayerWeapon.GreatSword, m_playerIdleWeapon_Prefab[3]);
        IdleWeaponObject.Add(PlayerWeapon.Umbrella, m_playerIdleWeapon_Prefab[4]);
    }

    public void SetWeapon(PlayerWeapon weaponType)
    {
        m_weaponType = weaponType;

        Component component = gameObject.GetComponent<IWeapon>() as Component;

        if(component != null)
        {
            Destroy(component);
        }

        switch(weaponType)
        {
            case PlayerWeapon.Sword:
                m_currentWeapon = gameObject.AddComponent<Sword>();
                break;
            case PlayerWeapon.Hammer:
                m_currentWeapon = gameObject.AddComponent<Hammer>();
                break;
            case PlayerWeapon.Dagger:
                m_currentWeapon = gameObject.AddComponent<Dagger>();
                break;
            case PlayerWeapon.GreatSword:
                m_currentWeapon = gameObject.AddComponent<GreatSword>();
                break;
            case PlayerWeapon.Umbrella:
                m_currentWeapon = gameObject.AddComponent<Umbrella>();
                break;
        }

        IdleWeaponActive(true);
    }

    public void IdleWeaponActive(bool active)
    {
        if (m_playerIdleWeapon_Prefab[(int)m_weaponType] != null)
        {
            if (active)
            {
                GetIdleWeaponObject(m_weaponType).SetActive(true);
            }
            else
            {
                GetIdleWeaponObject(m_weaponType).SetActive(false);
            }
        }
        else
            return;
    }

    public void UseWeaponActive(bool active)
    {
        if (m_playerUseWeapon_Prefab[(int)m_weaponType] != null)
        {
            if (active)
            {
                GetUseWeaponObject(m_weaponType).SetActive(true);
            }
            else
            {
                GetUseWeaponObject(m_weaponType).SetActive(false);
            }
        }
        else
            return;
    }

    private void AllDisableWeapon()
    {
        foreach (var weapon in m_playerUseWeapon_Prefab)
        {
            if (weapon != null)
            {
                weapon.SetActive(false);
            }
        }

        foreach (var weapon in m_playerIdleWeapon_Prefab)
        {
            if (weapon != null)
            {
                weapon.SetActive(false);
            }
        }
    }

    public GameObject GetUseWeaponObject(PlayerWeapon weapon)
    {
        return UseWeaponObject[weapon];
    }

    public GameObject GetIdleWeaponObject(PlayerWeapon weapon)
    {
        return IdleWeaponObject[weapon];
    }

    public WeaponData GetWeaponData(PlayerWeapon weapon)
    {
        return WeaponDataDictionary[weapon];
    }

    public void UseWeapon()
    {
        m_currentWeapon.UseWeapon();
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

