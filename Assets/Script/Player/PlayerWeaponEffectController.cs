using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum Effect
{
    Sword,
    Hammer,
    Dagger,
    GreatSword,
    Umbrella
}

public class PlayerWeaponEffectController : MonoBehaviour
{
    [Header("LeftDirectionValue")]
    [SerializeField] private float m_LeftDirectionValue;

    [Header("RightDirectionValue")]
    [SerializeField] private float m_RightDirectionValue;

    [Header("SwordEffect")]
    [SerializeField] private GameObject m_SwordEffect;
    [SerializeField] private Transform m_SwordEffectTransform;

    [Header("HammerEffect")]

    [Header("DaggerEffect")]

    [Header("GreatSwordEffect")]

    [Header("UmbrellaEffect")]

    [Header("WeaponMaterial")]
    [SerializeField] private Material m_SwordMaterial;

    private Dictionary<Effect, GameObject> EffectDic = new Dictionary<Effect, GameObject>();
    private Dictionary<PlayerWeapon, Material> WeaponMaterialDic = new Dictionary<PlayerWeapon, Material>(); 
    private Dictionary<PlayerWeapon, Color> WeaponColorDic = new Dictionary<PlayerWeapon, Color>();
    private WaitForSeconds m_effectDelayTime = new WaitForSeconds(0.1f);

    private float m_normalRange;
    private float m_chargeRange;

    private void Awake()
    {
        InitEffect();
        InitColor();
    }

    private void Start()
    {
        EventManager.Instance.AddEvent_ActiveType(WeaponActiveType, ColorChange);
    }

    private void InitEffect()
    {
        CreateSwordEffect();
    }

    public void InitColor()
    {
        WeaponMaterialDic.Add(PlayerWeapon.Sword, m_SwordMaterial);
        WeaponColorDic.Add(PlayerWeapon.Sword, m_SwordMaterial.GetColor("_Color"));
        //WeaponMaterialDic.Add(PlayerWeapon.Hammer, m_HammerMaterial);
        //WeaponMaterialDic.Add(PlayerWeapon.Dagger, m_DaggerMaterial);
        //WeaponMaterialDic.Add(PlayerWeapon.GreatSword, m_GreatSwordMaterial);
        //WeaponMaterialDic.Add(PlayerWeapon.Umbrella, m_UmbrellaMaterial);
    }

    public void SetEffectRange(float normalRange, float chargeRange)
    {
        m_normalRange = normalRange;
        m_chargeRange = chargeRange;
    }

    private GameObject GetEffect(Effect effect)
    {
        return EffectDic[effect];
    }

    public void SetMaterial(PlayerWeapon weapon, Color newColor)
    {
        WeaponMaterialDic[weapon].SetColor("_Color", newColor);
    }

    public Material GetMaterial(PlayerWeapon weapon)
    {
        return WeaponMaterialDic[weapon];
    }

    public Color GetColor(PlayerWeapon weapon)
    {
        return WeaponColorDic[weapon];  
    }

    private void CreateSwordEffect()
    {
        GameObject swordEffect = Instantiate(m_SwordEffect, m_SwordEffectTransform);
        VisualEffect swordVfx = swordEffect.GetComponent<VisualEffect>();
        swordVfx.Stop();
        EffectDic.Add(Effect.Sword, swordEffect);
        swordEffect.transform.localPosition = Vector3.zero;
    }

    private void WeaponActiveType(ActiveType type, bool isCharge)
    {
        if(type is ActiveType.Right)
        {
            ActiveSwordEffect_R(isCharge);
        }
        else
        {
            ActiveSwordEffect_L(isCharge);
        }
    }

    public void ActiveSwordEffect_L(bool isCharge)
    {
        float effectRange = isCharge ? m_chargeRange : m_normalRange;
        GameObject swordEffect = GetEffect(Effect.Sword);
        VisualEffect swordVfx = swordEffect.GetComponent<VisualEffect>();
        swordVfx.SetFloat("Size", effectRange);
        swordVfx.SetFloat("Diretion", m_LeftDirectionValue);
        StartCoroutine(ActiveDelay_L(swordVfx));
    }

    private IEnumerator ActiveDelay_L(VisualEffect swordVfx)
    {
        yield return m_effectDelayTime;
        swordVfx.Play();
    }

    public void ActiveSwordEffect_R(bool isCharge)
    {
        float effectRange = isCharge ? m_chargeRange : m_normalRange;
        GameObject swordEffect = GetEffect(Effect.Sword);
        VisualEffect swordVfx = swordEffect.GetComponent<VisualEffect>();
        swordVfx.SetFloat("Size", effectRange);
        swordVfx.SetFloat("Diretion", m_RightDirectionValue);
        StartCoroutine(ActiveDelay_R(swordVfx));
    }

    private IEnumerator ActiveDelay_R(VisualEffect swordVfx)
    {
        yield return m_effectDelayTime;
        swordVfx.Play();
    }

    private void ColorChange(bool isActive, PlayerWeapon weapon)
    {
        if (isActive)
        {
            SetNewColor(weapon);
        }
        else
        {
            ResetColor(weapon);
        }
    }

    public void SetNewColor(PlayerWeapon weapon)
    {
        Color currentColor = GetMaterial(weapon).GetColor("_Color");
        Color newColor = currentColor * Mathf.Pow(2f, 3f);
        Material weaponMaterial = GetMaterial(weapon);
        weaponMaterial.SetColor("_Color", newColor);
    }

    public void ResetColor(PlayerWeapon weapon)
    {
        Material weaponMaterial = GetMaterial(weapon);
        weaponMaterial.SetColor("_Color",GetColor(weapon));
    }


}
