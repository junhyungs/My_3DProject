using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("PlayerIdleWeapon")]
    [SerializeField] private GameObject[] m_IdleObject;

    [Header("PlayerLeftWeapon")]
    [SerializeField] private GameObject[] m_LeftObject;

    [Header("PlayerRightWeapon")]
    [SerializeField] private GameObject[] m_RightObject;

    [Header("PlayerSkillWeapon")]
    [SerializeField] private GameObject[] m_SkillObject;

    private IWeapon m_currentWeapon;
    private PlayerWeapon m_weaponType;
    private Animator m_weaponAnimation;

    private WaitForSeconds m_deactiveRightTime = new WaitForSeconds(0.4f);
    private WaitForSeconds m_deactiveLeftTime = new WaitForSeconds(0.4f);

    public IWeapon CurrentWeapon => m_currentWeapon;
    public PlayerWeapon WeaponType => m_weaponType;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_weaponAnimation = GetComponent<Animator>();
        OnDisableWeaponObject();
        m_weaponType = PlayerWeapon.Sword;
        SetWeapon(m_weaponType);
    }

    public void SetWeapon(PlayerWeapon weaponType)
    {
        m_weaponType = weaponType;

        Component component = gameObject.GetComponent<IWeapon>() as Component;

        if (component != null)
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

        WeaponManager.Instance.SetCurrentWeapon(m_weaponType);
        ActiveIdleWeaponObject(true);
    }

    public void ActiveIdleWeaponObject(bool isActive)
    {
        m_IdleObject[(int)m_weaponType].SetActive(isActive);
    }

    public void ActiveLeftWeaponObject()
    {
        m_LeftObject[(int)m_weaponType].SetActive(true);

        ActiveIdleWeaponObject(false);
        StartCoroutine(DeactiveLeftWeaponObject());
    }

    public void ActiveRightWeaponObject()
    {
        m_RightObject[(int)m_weaponType].SetActive(true);

        ActiveIdleWeaponObject(false);
        StartCoroutine(DeactiveRightWeaponObject());
    }

    public void ActiveSkillWeaponObject(PlayerSkill skillName, bool isPressed)
    {
        m_SkillObject[(int)skillName].SetActive(isPressed);
    }

    public IEnumerator DeactiveRightWeaponObject()
    {
        yield return m_deactiveRightTime;
        m_RightObject[(int)m_weaponType].SetActive(false);
        ActiveIdleWeaponObject(true);
        m_weaponAnimation.SetBool("NextAttack", true);
    }

    public IEnumerator DeactiveLeftWeaponObject()
    {
        yield return m_deactiveLeftTime;
        m_LeftObject[(int)m_weaponType].SetActive(false);
        ActiveIdleWeaponObject(true);
        m_weaponAnimation.SetBool("NextAttack", true);
    }

    private void OnDisableWeaponObject()
    {
        foreach(var idleWeapon in m_IdleObject)
        {
            idleWeapon.SetActive(false);
        }

        foreach(var leftWeapon in m_LeftObject)
        {
            leftWeapon.SetActive(false);
        }

        foreach(var rightWeapon in m_RightObject)
        {
            rightWeapon.SetActive(false);
        }

        foreach(var skillWeapon in m_SkillObject)
        {
            skillWeapon.SetActive(false);
        }
    }

    public void UseWeapon()
    {
        m_weaponAnimation.SetTrigger("Attack");
        m_weaponAnimation.SetBool("NextAttack", false);
        m_currentWeapon.UseWeapon();
    }

}
