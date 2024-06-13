using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("PlayerWeapon")]
    [SerializeField] private GameObject[] m_playerWeapon;

    private IWeapon m_weapon;
    private PlayerWeapon m_currentWeapon;
    private GameObject m_currentWeaponObj;

    private void Start()
    {
        AllWeaponDisable();
        m_currentWeaponObj = m_playerWeapon[0];
        SetWeapon(PlayerWeapon.Sword);
    }


    private void SetWeapon(PlayerWeapon weapon)
    {   
        m_currentWeapon = weapon;

        Component component = gameObject.GetComponent<IWeapon>() as Component;

        if(component != null)
        {
            Destroy(component);
        }

        switch (weapon)
        {
            case PlayerWeapon.Sword:
                m_weapon = gameObject.AddComponent<Sword>();
                break;
            case PlayerWeapon.Bow:
                m_weapon = gameObject.AddComponent<Bow>();
                break;
        }

        if (m_currentWeaponObj != null)
        {
            m_currentWeaponObj.SetActive(false);

            m_currentWeaponObj = m_playerWeapon[(int)weapon];

            m_currentWeaponObj.SetActive(true);
        }

    }

    private void AllWeaponDisable()
    {
        foreach(var weapon in m_playerWeapon)
        {
            if(weapon != null)
            {
                weapon.SetActive(false);
            }
        }
    }

}
