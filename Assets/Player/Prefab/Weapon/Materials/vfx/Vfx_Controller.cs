using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Vfx_Controller : MonoBehaviour
{
    [Header("LeftDirectionValue")]
    [SerializeField] private float m_leftDirectionValue;

    [Header("RightDirectionValue")]
    [SerializeField] private float m_rightDirectionValue;

    [Header("NormalAttactSize")]
    [SerializeField] private float m_normalAttackSize;

    [Header("ChargeAttackSize")]
    [SerializeField] private float m_chargeAttackSize;

    private VisualEffect m_vEffect;
    
    

    private void Start()
    {
        m_vEffect = GetComponent<VisualEffect>();
        m_vEffect.Stop();
    }

    public void LeftWeaponEffect(bool isCharge)
    {
        float effectSize = isCharge ? m_chargeAttackSize : m_normalAttackSize;

        m_vEffect.SetFloat("Size", effectSize);
        m_vEffect.SetFloat("Diretion", m_leftDirectionValue);
        m_vEffect.Play();
    }

    public void RightWeaponEffect(bool isCharge)
    {
        float effectSize = isCharge ? m_chargeAttackSize : m_normalAttackSize;

        m_vEffect.SetFloat("Size", effectSize);
        m_vEffect.SetFloat("Diretion", m_rightDirectionValue);
        m_vEffect.Play();
    }


}
