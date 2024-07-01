using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour, IOnColliderEvent
{
    
    [Header("LeftWeapon")]
    [SerializeField] private GameObject[] LeftObject;

    [Header("RightWeapon")]
    [SerializeField] private GameObject[] RightObject;

    [Header("LeftChargeWeapon")]
    [SerializeField] private GameObject[] LeftChargeObject;

    [Header("RightChargeWeapon")]
    [SerializeField] private GameObject[] RightChargeObject;

    private Action m_Oncollider;
    private Action m_Offcollider;
    private Dictionary<int, Action<bool>> Animation_ActionDic = new Dictionary<int, Action<bool>>();

    private Animator m_weaponAnimation;
    [SerializeField]
    private PlayerWeaponEffectController m_effect;

    public Dictionary<int, Action<bool>> ActiveWeaponDic => Animation_ActionDic;

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
        EventManager.Instance.RegisterActiveTriggerColliderEvent(this);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        m_weaponAnimation = GetComponent<Animator>();
        OnDisableWeaponObject();
        Init_AnimationDic();
    }

    private void Init_AnimationDic()
    {
        Animation_ActionDic.Add(m_Slash_Light_L, ActiveRightWeapon);
        Animation_ActionDic.Add(m_Slash_Light_R, ActiveLeftWeapon);
        Animation_ActionDic.Add(m_Slash_Light_Last, ActiveRightWeapon);
        Animation_ActionDic.Add(m_Charge_slash_L, ActiveChargeLeftWeapon);
        Animation_ActionDic.Add(m_Charge_slash_R, ActiveChargeRightWeapon);
    }
   

    public void ActiveLeftWeapon(bool isCharge)
    {
        PlayerWeapon currentWeapon = WeaponManager.Instance.GetcurrentWeapon();

        LeftChargeObject[(int)currentWeapon].SetActive(false);
        LeftObject[(int)currentWeapon].SetActive(true);
        m_effect.ActiveSwordEffect_L(isCharge);
        WeaponManager.Instance.ActiveIdleWeapon(false);
        m_Oncollider?.Invoke();
    }

    public void ActiveChargeLeftWeapon(bool isCharge)
    {
        PlayerWeapon currentWeapon = WeaponManager.Instance.GetcurrentWeapon();

        m_effect.SetNewColor(currentWeapon);
        LeftChargeObject[(int)currentWeapon].SetActive(true);
        WeaponManager.Instance.ActiveIdleWeapon(false);
    }

    public void ActiveRightWeapon(bool isCharge)
    {
        PlayerWeapon currentWeapon = WeaponManager.Instance.GetcurrentWeapon();

        RightChargeObject[(int)currentWeapon].SetActive(false);
        RightObject[(int)currentWeapon].SetActive(true);
        m_effect.ActiveSwordEffect_R(isCharge);
        WeaponManager.Instance.ActiveIdleWeapon(false);
        m_Oncollider?.Invoke();
    }
    public void ActiveChargeRightWeapon(bool isCharge)
    {
        PlayerWeapon currentWeapon = WeaponManager.Instance.GetcurrentWeapon();

        m_effect.SetNewColor(currentWeapon);
        RightChargeObject[(int)currentWeapon].SetActive(true);
        WeaponManager.Instance.ActiveIdleWeapon(false);
    }

    public void DeActiveRightWeapon()
    {
        PlayerWeapon currentWeapon = WeaponManager.Instance.GetcurrentWeapon();

        m_effect.ResetColor(currentWeapon);
        RightObject[(int)currentWeapon].SetActive(false);
        WeaponManager.Instance.ActiveIdleWeapon(true);
        m_weaponAnimation.SetBool("NextAttack", true);
        m_Offcollider?.Invoke();
    }

    public void DeActiveLeftWeapon()
    {
        PlayerWeapon currentWeapon = WeaponManager.Instance.GetcurrentWeapon();

        m_effect.ResetColor(currentWeapon);
        LeftObject[(int)currentWeapon].SetActive(false);
        WeaponManager.Instance.ActiveIdleWeapon(true);
        m_weaponAnimation.SetBool("NextAttack", true);
        m_Offcollider?.Invoke();
    }

    public void ChargeAttackReset()
    {
        PlayerWeapon currentWeapon = WeaponManager.Instance.GetcurrentWeapon();

        m_effect.ResetColor(currentWeapon);
        RightChargeObject[(int)currentWeapon].SetActive(false);
        LeftChargeObject[(int)currentWeapon].SetActive(false);
    }


    private void OnDisableWeaponObject()
    {
        foreach(var leftWeapon in LeftObject)
        {
            leftWeapon.SetActive(false);
        }

        foreach(var rightWeapon in RightObject)
        {
            rightWeapon.SetActive(false);
        }
    }

    public void Attack()
    {
        m_weaponAnimation.SetTrigger("Attack");
        m_weaponAnimation.SetBool("NextAttack", false);
    }

    public void OnCollider(bool isAddEvent, Action callBack)
    {
        if (isAddEvent)
        {
            m_Oncollider += callBack;
        }
        else
        {
            m_Oncollider -= callBack;
        }

    }

    public void OffCollider(bool isAddEvent, Action callBack)
    {
        if(isAddEvent)
        {
            m_Offcollider += callBack;
        }
        else
        {
            m_Offcollider -= callBack;
        }
    }
}
