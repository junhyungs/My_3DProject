using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomObject : MonoBehaviour, IDamaged
{
    [Header("MeshRenderer")]
    [SerializeField] private MeshRenderer _renderer;
    [Header("OriginalMaterial")]
    [SerializeField] private Material _originalMaterial;

    private Coroutine _intensityChange;
    private WaitForSeconds _intensityTime;
    private Material _copyMaterial;

    private float _baseValue = 2f;
    private float _power = 3f;

    private void Start()
    {
        _intensityTime = new WaitForSeconds(0.1f);

        SetMaterial();
    }

    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_originalMaterial);
        _renderer.material = _copyMaterial;
    }

    public void TakeDamage(float damage)
    {
        if(_intensityChange != null)
        {
            return;
        }

        _intensityChange = StartCoroutine(IntensityChange());
    }

    private IEnumerator IntensityChange()
    {
        Color color = _copyMaterial.GetColor("_Color");

        Color intensityUpColor = color * Mathf.Pow(_baseValue, _power);

        _copyMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _copyMaterial.SetColor("_Color", color);

        _intensityChange = null;
    }
}
