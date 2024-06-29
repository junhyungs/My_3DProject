using System;
using System.Collections.Generic;
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
    [Header("IdleWeapon")]
    [SerializeField] private GameObject[] IdleObject;

    private Dictionary<PlayerWeapon, WeaponData> WeaponDataDic = new Dictionary<PlayerWeapon, WeaponData>();
    private IWeapon m_currentWeapon;
    private IWeaponEvent m_weaponEvent;
    private IOnColliderEvent m_colliderEvent;
    private PlayerWeapon m_weaponType;

    private void Awake()
    {
        InitializeWeaponData();
        OnDisableIdleWeaponObject();
        m_weaponType = PlayerWeapon.Sword;
        SetWeapon(m_weaponType);
    }

    public void InitializeWeaponData()
    {
        InitWeapon(PlayerWeapon.Sword, "Sword");
        InitWeapon(PlayerWeapon.Hammer, "Hammer");
        InitWeapon(PlayerWeapon.Dagger, "Dagger");
        InitWeapon(PlayerWeapon.GreatSword, "GreatSword");
        InitWeapon(PlayerWeapon.Umbrella, "Umbrella");
    }

    private void InitWeapon(PlayerWeapon weaponType, string weaponName)
    {
        var weaponData = DataManager.Instance.GetWeaponData(weaponName);
        WeaponData data = new WeaponData(
            weaponData.WeaponName,
            weaponData.AttackPower,
            weaponData.ChargeAttackPower,
            weaponData.NormalEffectRange,
            weaponData.ChargeEffectRange,
            weaponData.NormalAttackRange,
            weaponData.ChargeAttackRange
            );
        
        
        WeaponDataDic.Add(weaponType, data);
    }
  
    public void SetWeapon(PlayerWeapon weaponType)
    {
        ActiveIdleWeapon(false);

        m_weaponType = weaponType;

        Component component = gameObject.GetComponent<IWeapon>() as Component;

        if(component != null)
        {
            Destroy(component);
        }

        switch (weaponType)
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
        
        
        ActiveIdleWeapon(true);
    }

    public void ActiveIdleWeapon(bool active)
    {
        IdleObject[(int)m_weaponType].SetActive(active);
    }

    //RegisterEvent---------------------------------------------------
    public void RegisterWeaponEvent(IWeaponEvent weaponEvent)
    {
        m_weaponEvent = weaponEvent;
    }

    public void RegisterColliderEvent(IOnColliderEvent onColliderEvent)
    {
        m_colliderEvent = onColliderEvent;
    }
    //RegisterEvent---------------------------------------------------

    //AddEvent--------------------------------------------------------
    public void AddData(bool isAddEvent, Action<float, float, Vector3, Vector3> callBack)
    {
        m_weaponEvent.AddWeaponData(isAddEvent, callBack);
    }

    public void AddOnColliderEvent(bool isAddEvent, Action callBack)
    {
        m_colliderEvent.OnCollider(isAddEvent, callBack);
    }

    public void AddOffColliderEvent(bool isAddEvent, Action callBack)
    {
        m_colliderEvent.OffCollider(isAddEvent, callBack);
    }

    public void AddWeaponRangeEvent(bool isAddEvent, Action<bool> callBack)
    {
        m_weaponEvent.AddUseWeaponEvent(isAddEvent, callBack);
    }
    //AddEvent--------------------------------------------------------

    public WeaponData GetWeaponData(PlayerWeapon weaponType)
    {
        return WeaponDataDic[weaponType];
    }

    public PlayerWeapon GetcurrentWeapon()
    {
        return m_weaponType;
    }

    public void UseWeapon(bool isCharge)
    {
        m_currentWeapon.UseWeapon(isCharge);
    }

    private void OnDisableIdleWeaponObject()
    {
        foreach(var idleWeapon in IdleObject)
        {
            idleWeapon.SetActive(false);
        }
    }
  
}

public struct WeaponData
{
    public string m_weaponName { get; }
    public float m_defaultPower { get; }
    public float m_chargePower { get; }
    public float m_defaultEffectRange { get; }
    public float m_chargeEffectRange { get; }
    public Vector3 m_defaultAttackRange { get; }
    public Vector3 m_chargeAttackRange { get; }
    public WeaponData(string weaponName, float defaultPower, float chargePower, float defaultEffectRange, float chargeEffectRange, Vector3 defaultAttackRange, Vector3 chargeAttackRange)
    {
        m_weaponName =  weaponName;
        m_defaultPower = defaultPower;
        m_chargePower = chargePower;
        m_defaultEffectRange = defaultEffectRange;
        m_chargeEffectRange = chargeEffectRange;
        m_defaultAttackRange = defaultAttackRange;
        m_chargeAttackRange = chargeAttackRange;
    }
}

