using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("PlayerIdleWeapon")]
    [SerializeField] private GameObject[] m_IdleObject;

    [Header("PlayerLeftWeapon")]
    [SerializeField] private GameObject[] m_LeftObject;

    [Header("PlayerChargeLeftWeapon")]
    [SerializeField] private GameObject[] m_LeftChargeObject;

    [Header("PlayerRightWeapon")]
    [SerializeField] private GameObject[] m_RightObject;

    [Header("PlayerChargeRightWeapon")]
    [SerializeField] private GameObject[] m_RightChargeObject;

    [Header("PlayerSkillWeapon")]
    [SerializeField] private GameObject[] m_SkillObject;

    [Header("SwordEffect")]
    [SerializeField] private Vfx_Controller m_vEffect;
    [SerializeField] private MeshCollider m_NormalAttackCollider;
    [SerializeField] private MeshCollider m_ChargeAttackCollider;

    [Header("SwordMaterial")]
    [SerializeField] private Material m_swordMaterial;
    private Color m_materialColor;

    private IWeapon m_currentWeapon;
    private PlayerWeapon m_weaponType;

    private WaitForSeconds m_deactiveRightTime = new WaitForSeconds(0.4f);
    private WaitForSeconds m_deactiveLeftTime = new WaitForSeconds(0.4f);

    private Animator m_weaponAnimation;
    private Dictionary<int, Action<bool>> Animation_ActionDic = new Dictionary<int, Action<bool>>();

    public Dictionary<int, Action<bool>> ActiveWeaponDic => Animation_ActionDic;
    public IWeapon CurrentWeapon => m_currentWeapon;
    public PlayerWeapon WeaponType => m_weaponType;

    #region Animation.StringToHash
    private int m_Slash_Light_L = Animator.StringToHash("Slash_Light_L");
    private int m_Slash_Light_R = Animator.StringToHash("Slash_Light_R");
    private int m_Slash_Light_Last = Animator.StringToHash("Slash_Light_L_Last");
    private int m_Charge_slash_L = Animator.StringToHash("Charge_slash_L");
    private int m_Charge_slash_R = Animator.StringToHash("Charge_slash_R");
    private int m_Charge_MaxL = Animator.StringToHash("Charge_max_L");
    private int m_Charge_MaxR = Animator.StringToHash("Charge_max_R");
    #endregion

    private void Awake()
    {
        Init();
        Init_AnimationDic();
    }

    private void Init()
    {
        m_weaponAnimation = GetComponent<Animator>();
        m_materialColor = m_swordMaterial.GetColor("_Color");
        m_NormalAttackCollider.enabled = false;
        m_ChargeAttackCollider.enabled = false;
        OnDisableWeaponObject();
        m_weaponType = PlayerWeapon.Sword;
        SetWeapon(m_weaponType);
    }

    private void Init_AnimationDic()
    {
        Animation_ActionDic.Add(m_Slash_Light_L, ActiveRightWeaponObject);
        Animation_ActionDic.Add(m_Slash_Light_R, ActiveLeftWeaponObject);
        Animation_ActionDic.Add(m_Slash_Light_Last, ActiveRightWeaponObject);
        Animation_ActionDic.Add(m_Charge_slash_L, ActiveChargeLeftWeaponObject);
        Animation_ActionDic.Add(m_Charge_slash_R, ActiveChargeRightWeaponObject);
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

    public void ActiveLeftWeaponObject(bool isCharge)
    {
        m_LeftChargeObject[(int)m_weaponType].SetActive(false);
        m_LeftObject[(int)m_weaponType].SetActive(true);
        m_vEffect.LeftWeaponEffect(isCharge);
        ActiveIdleWeaponObject(false);
        OnCollider(isCharge);
        StartCoroutine(DeactiveLeftWeaponObject());
    }

    public void ActiveChargeLeftWeaponObject(bool isCharge)
    {
        Color color = m_swordMaterial.GetColor("_Color");
        Color newColor = color * Mathf.Pow(2f, 3f);

        m_swordMaterial.SetColor("_Color", newColor);

        m_LeftChargeObject[(int)m_weaponType].SetActive(true);
        ActiveIdleWeaponObject(false);
    }

    public void ActiveRightWeaponObject(bool isCharge)
    {
        m_RightChargeObject[(int)m_weaponType].SetActive(false);
        m_RightObject[(int)m_weaponType].SetActive(true);
        m_vEffect.RightWeaponEffect(isCharge);
        ActiveIdleWeaponObject(false);
        OnCollider(isCharge);
        StartCoroutine(DeactiveRightWeaponObject());
    }
    public void ActiveChargeRightWeaponObject(bool isCharge)
    {
        Color color = m_swordMaterial.GetColor("_Color");
        Color newColor = color * Mathf.Pow(2f, 3f);

        m_swordMaterial.SetColor("_Color",newColor);

        m_RightChargeObject[(int)m_weaponType].SetActive(true);
        ActiveIdleWeaponObject(false);
    }

    public void ActiveSkillWeaponObject(PlayerSkill skillName, bool isPressed)
    {
        m_SkillObject[(int)skillName].SetActive(isPressed);
    }

    public IEnumerator DeactiveRightWeaponObject()
    {
        yield return m_deactiveRightTime;
        OffCollider();
        m_swordMaterial.SetColor("_Color", m_materialColor);
        m_RightObject[(int)m_weaponType].SetActive(false);
        ActiveIdleWeaponObject(true);
        m_weaponAnimation.SetBool("NextAttack", true);
    }

    public IEnumerator DeactiveLeftWeaponObject()
    {
        yield return m_deactiveLeftTime;
        OffCollider();
        m_swordMaterial.SetColor("_Color", m_materialColor);
        m_LeftObject[(int)m_weaponType].SetActive(false);
        ActiveIdleWeaponObject(true);
        m_weaponAnimation.SetBool("NextAttack", true);
    }

    public void ChargeAttackReset()
    {
        m_swordMaterial.SetColor("_Color", m_materialColor);
        m_RightChargeObject[(int)m_weaponType].SetActive(false);
        m_LeftChargeObject[(int)m_weaponType].SetActive(false);
    }

    private void OnCollider(bool isCharge)
    {
        if (isCharge)
        {
            m_ChargeAttackCollider.enabled = true;
            return;
        }

        m_NormalAttackCollider.enabled = true;
    }

    private void OffCollider()
    {
        m_ChargeAttackCollider.enabled = false;
        m_NormalAttackCollider.enabled = false;
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

    public void UseWeapon(bool isCharge)
    {
        m_weaponAnimation.SetTrigger("Attack");
        m_weaponAnimation.SetBool("NextAttack", false);
        m_currentWeapon.UseWeapon(isCharge);
    }

}
