using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("IdleWeapon")]
    [SerializeField] private GameObject[] _idleObject;
    [Header("LeftWeapon")]
    [SerializeField] private GameObject[] LeftObject;
    [Header("RightWeapon")]
    [SerializeField] private GameObject[] RightObject;
    [Header("LeftChargeWeapon")]
    [SerializeField] private GameObject[] LeftChargeObject;
    [Header("RightChargeWeapon")]
    [SerializeField] private GameObject[] RightChargeObject;

    private Dictionary<int, Action<bool>> Animation_ActionDic = new Dictionary<int, Action<bool>>();
    private Animator m_weaponAnimation;

    private IWeapon _currentWeapon;
    private PlayerWeapon _currentWeaponType;

    public Dictionary<int, Action<bool>> ActiveWeaponDic => Animation_ActionDic;

    private PlayerData _data;
    private PlayerWeaponEffectController _effectController;

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
        InitializeWeaponController();
    }

    private void OnEnable()
    {
        if(_currentWeapon != null)
        {
            ResetWeapon();
        }
    }

    private void Start()
    {
        StartCoroutine(GetPlayerData());
    }

    private void InitializeWeaponController()
    {
        WeaponManager.Instance.SetWeaponController(this);
        m_weaponAnimation = gameObject.GetComponent<Animator>();
        _effectController = gameObject.GetComponent<PlayerWeaponEffectController>();
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

    private IEnumerator GetPlayerData()
    {
        yield return new WaitWhile(() => { return DataManager.Instance.GetData("P101") == null; });

        var data = DataManager.Instance.GetData("P101") as PlayerData;

        _data = data;

        SetWeapon(PlayerWeapon.Sword);
    }

    public void TakeDamageWeapon()
    {
        ResetWeapon();
        ChargeAttackReset();
    }
   
    public void ActiveLeftWeapon(bool isCharge)
    {
        LeftChargeObject[(int)_currentWeaponType].SetActive(false);

        LeftObject[(int)_currentWeaponType].SetActive(true);

        _effectController.ActiveSwordEffect_L(isCharge, _currentWeaponType);

        ActiveIdleWeapon(false);
    }

    public void ActiveChargeLeftWeapon(bool isCharge)
    {
        LeftChargeObject[(int)_currentWeaponType].SetActive(true);     
        ActiveIdleWeapon(false);
    }

    public void ActiveRightWeapon(bool isCharge)
    {
        RightChargeObject[(int)_currentWeaponType].SetActive(false);
        RightObject[(int)_currentWeaponType].SetActive(true);
        _effectController.ActiveSwordEffect_R(isCharge, _currentWeaponType);
        ActiveIdleWeapon(false);
    }

    public void ActiveChargeRightWeapon(bool isCharge)
    {
        RightChargeObject[(int)_currentWeaponType].SetActive(true);
        ActiveIdleWeapon(false);
    }

    public void DeActiveRightWeapon()
    {
        RightObject[(int)_currentWeaponType].SetActive(false);
        ActiveIdleWeapon(true);
        m_weaponAnimation.SetBool("NextAttack", true);
    }

    public void DeActiveLeftWeapon()
    {
        LeftObject[(int)_currentWeaponType].SetActive(false);    
        ActiveIdleWeapon(true);
        m_weaponAnimation.SetBool("NextAttack", true);
    }

    public void ChargeAttackReset()
    {
        RightChargeObject[(int)_currentWeaponType].SetActive(false);
        LeftChargeObject[(int)_currentWeaponType].SetActive(false);
    }

    private void ActiveIdleWeapon(bool active)
    {
        _idleObject[(int)_currentWeaponType].SetActive(active);
    }

    private void ResetWeapon()
    {
        RightObject[(int)_currentWeaponType].SetActive(false);
        LeftObject[(int)_currentWeaponType].SetActive(false);

        ActiveIdleWeapon(true);
    }

    private void OnDisableWeaponObject()
    {
        foreach(var idleWeapon in _idleObject)
        {
            idleWeapon.SetActive(false);
        }

        foreach(var leftWeapon in LeftObject)
        {
            leftWeapon.SetActive(false);
        }

        foreach(var rightWeapon in RightObject)
        {
            rightWeapon.SetActive(false);
        }
    }

    public void SetWeapon(PlayerWeapon newWeapon)
    {
        ActiveIdleWeapon(false);

        _currentWeaponType = newWeapon;

        Component component = gameObject.GetComponent<IWeapon>() as Component;

        if(component != null)
        {
            Destroy(component);
        }

        switch(newWeapon)
        {
            case PlayerWeapon.Sword:
                _currentWeapon = gameObject.AddComponent<Sword>();
                _effectController.ChageColor(PlayerWeapon.Sword);
                StartCoroutine(WeaponManager.Instance.LoadWeaponData("W101", _currentWeapon, _data));
                break;
            case PlayerWeapon.Hammer:
                _currentWeapon = gameObject.AddComponent<Hammer>();
                _effectController.ChageColor(PlayerWeapon.Hammer);
                StartCoroutine(WeaponManager.Instance.LoadWeaponData("W102", _currentWeapon, _data));
                break;
            case PlayerWeapon.Dagger:
                _currentWeapon = gameObject.AddComponent<Dagger>();
                _effectController.ChageColor(PlayerWeapon.Dagger);
                StartCoroutine(WeaponManager.Instance.LoadWeaponData("W103", _currentWeapon, _data));
                break;
            case PlayerWeapon.GreatSword:
                _currentWeapon = gameObject.AddComponent<GreatSword>();
                _effectController.ChageColor(PlayerWeapon.GreatSword);
                StartCoroutine(WeaponManager.Instance.LoadWeaponData("W104", _currentWeapon, _data));
                break;
            case PlayerWeapon.Umbrella:
                _currentWeapon = gameObject.AddComponent<Umbrella>();
                _effectController.ChageColor(PlayerWeapon.Umbrella);
                StartCoroutine(WeaponManager.Instance.LoadWeaponData("W105", _currentWeapon, _data));
                break;
        }

        WeaponManager.Instance.CurrentWeapon = newWeapon;

        ActiveIdleWeapon(true);
    }

    public void Attack()
    {
        m_weaponAnimation.SetTrigger("Attack");
        m_weaponAnimation.SetBool("NextAttack", false);
    }

    public void OnHit(bool isCharge)
    {
        _currentWeapon.UseWeapon(isCharge);
    }
}
