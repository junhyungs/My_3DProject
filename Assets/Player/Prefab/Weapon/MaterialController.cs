using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [Header("SwordEffectMaterial")]
    [SerializeField] private Material m_swordEffectMaterial;

    private Coroutine m_swordEffectCor;
    private float m_swordEffectAmountTime = 1.5f;

    public void OnSwordEffectMaterial()
    {
        if(m_swordEffectCor != null)
        {
            StopCoroutine(m_swordEffectCor); ;
        }

        m_swordEffectCor = StartCoroutine(OnSwordEffect());
    }

    private IEnumerator OnSwordEffect()
    {
        float time = 0f;
        float amountValue = 0.1f;

        while(time < m_swordEffectAmountTime)
        {
            m_swordEffectMaterial.SetFloat("_Value", amountValue);
            amountValue -= 0.001f;
            yield return null;
            time += Time.deltaTime;
        }

        m_swordEffectCor = null;
    }

}
