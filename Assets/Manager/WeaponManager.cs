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
    private Dictionary<PlayerWeapon, Weapon> WeaponDataDic = new Dictionary<PlayerWeapon, Weapon>();
    private PlayerWeapon m_currentWeapon;

    private void Awake()
    {
        InitWeaponData();
    }

    private void InitWeaponData()
    {
        WeaponDataDictionary.Add(PlayerWeapon.Sword, new WeaponData(1, 3, 0.7f,1.0f,new Vector3(0.7f,0.7f,0.7f),new Vector3(1.0f,0.7f,1.0f)));
        WeaponDataDictionary.Add(PlayerWeapon.Hammer, new WeaponData(1, 3, 0.7f, 1.0f, new Vector3(0.7f, 0.7f, 0.7f), new Vector3(1.0f, 0.7f, 1.0f)));
        WeaponDataDictionary.Add(PlayerWeapon.Dagger, new WeaponData(1, 3, 0.7f, 1.0f, new Vector3(0.7f, 0.7f, 0.7f), new Vector3(1.0f, 0.7f, 1.0f)));
        WeaponDataDictionary.Add(PlayerWeapon.GreatSword, new WeaponData(1, 3, 0.7f, 1.0f, new Vector3(0.7f, 0.7f, 0.7f), new Vector3(1.0f, 0.7f, 1.0f)));
        WeaponDataDictionary.Add(PlayerWeapon.Umbrella, new WeaponData(1, 3, 0.7f, 1.0f, new Vector3(0.7f, 0.7f, 0.7f), new Vector3(1.0f, 0.7f, 1.0f)));
    }

    private void Start()
    {
        //웨폰 객체를 보관하는 코드로 변경
        
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
    public int m_attackPower { get; }
    public int m_chargeAttackPower { get; }
    public float m_normalEffectRange { get; }
    public float m_chargeEffectRange { get; }
    public Vector3 m_normalAttackRange { get; }
    public Vector3 m_chargeAttackRange { get; }
    public WeaponData(int attackPower, int chargeAttackPower, float normalEffectRange, float chargeEffectRange, Vector3 normalAttackRange, Vector3 chargeAttackRange)
    {
        m_attackPower = attackPower;
        m_chargeAttackPower = chargeAttackPower;
        m_normalEffectRange = normalEffectRange;
        m_chargeEffectRange = chargeEffectRange;
        m_normalAttackRange = normalAttackRange;
        m_chargeAttackRange = chargeAttackRange;
    }
}

