using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Vfx_Controller : MonoBehaviour
{
    private VisualEffect m_vEffect;
    private float m_leftDirectionValue = 4.5f;
    private float m_rightDirectionValue = -9.5f;
    private float m_normalAttackSize = 0.7f;
    private float m_chargeAttackSize = 1.5f;

    private void Start()
    {
        m_vEffect = GetComponent<VisualEffect>();
        m_vEffect.Stop();
    }

    public void LeftWeaponEffect()
    {
        m_vEffect.SetFloat("Size", m_normalAttackSize);
        m_vEffect.SetFloat("Diretion", m_leftDirectionValue);
        m_vEffect.Play();
    }

    public void RightWeaponEffect()
    {
        m_vEffect.SetFloat("Size", m_normalAttackSize);
        m_vEffect.SetFloat("Diretion", m_rightDirectionValue);
        m_vEffect.Play();
    }


}
