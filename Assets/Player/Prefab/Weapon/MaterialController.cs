using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material m_material;

    private float dissolveAmount = 0.1f;
    private bool dissolve = false;
    private float m_time = 3.0f;

    private void OnEnable()
    {
        dissolveAmount = 0.1f;

        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        float t = 0;

        while( t < m_time)
        {
            m_material.SetFloat("_Float", dissolveAmount);
            dissolveAmount -= 0.001f;
            yield return null;
            t += Time.deltaTime;
        }

        this.gameObject.SetActive(false);
    }

}
