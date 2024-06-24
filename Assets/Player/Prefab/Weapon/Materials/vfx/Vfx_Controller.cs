using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Vfx_Controller : MonoBehaviour
{
    [Header("LeftDirectionValue")]
    [SerializeField] private float m_leftDirectionValue;

    [Header("RightDirectionValue")]
    [SerializeField] private float m_rightDirectionValue;

    private VisualEffect m_vEffect;
    private float m_normalAttackSize;
    private float m_chargeAttackSize;
    private bool isCharge;
    
    public float NormalAttackSize
    {
        get { return m_normalAttackSize;}
        set { m_normalAttackSize = value;}
    }

    public float ChargeAttackSize
    {
        get { return m_chargeAttackSize; }
        set { m_chargeAttackSize = value;}
    }

    private void Start()
    {
        m_vEffect = GetComponent<VisualEffect>();
        m_vEffect.Stop();
    }

    public void IsCharge(bool isCharge)
    {
        this.isCharge = isCharge;
    }

    public void LeftWeaponEffect()
    {
        float range = isCharge ? m_chargeAttackSize : m_normalAttackSize;
        Debug.Log(range);
        m_vEffect.SetFloat("Size", range);
        m_vEffect.SetFloat("Diretion", m_leftDirectionValue);
        m_vEffect.Play();
    }

    public void RightWeaponEffect()
    {
        float range = isCharge ? m_chargeAttackSize : m_normalAttackSize;
        Debug.Log(range);
        m_vEffect.SetFloat("Size", range);
        m_vEffect.SetFloat("Diretion", m_rightDirectionValue);
        m_vEffect.Play();
    }


}
