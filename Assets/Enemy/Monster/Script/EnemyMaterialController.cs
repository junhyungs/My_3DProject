using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMaterialController : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material m_originalMaterial;

    private SkinnedMeshRenderer m_meshRenderer;
    private Material m_copyMaterial;

    private Color m_saveColor;
    private float m_value = 0.5f;
    private bool isEnd;
    
    public bool IsEnd {  get { return isEnd; } }

    private void Start()
    {
        m_copyMaterial = Instantiate(m_originalMaterial);
        m_meshRenderer = GetComponent<SkinnedMeshRenderer>();
        m_meshRenderer.material = m_copyMaterial;
        m_saveColor = m_copyMaterial.GetColor("_Color");
    }

    public IEnumerator IntensityChange()
    {
        Color color = m_copyMaterial.GetColor("_Color");
        Color newColor = color * Mathf.Pow(2f, 2f);
        m_copyMaterial.SetColor("_Color", newColor);

        yield return new WaitForSeconds(0.1f);

        m_copyMaterial.SetColor("_Color", m_saveColor);
    }
    public IEnumerator DieMaterial()
    {
        float timer = 2.0f;

        while(timer > 0f)
        {
            m_value -= 0.01f;
            m_copyMaterial.SetFloat("_Float", m_value);
            yield return null;
            timer -= Time.deltaTime;
        }
        
        isEnd = true;
    }
}
