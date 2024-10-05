using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Vine
{
    Left,
    Right
}

public class ForestMotherVine : MonoBehaviour, IDamged, ISendVineEvent
{
    [Header("VineType")]
    [SerializeField] private Vine _vineType;
    [Header("VineSkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [Header("Material")]
    [SerializeField] private Material _material;

    private Material _copyMaterial;
    private WaitForSeconds _intensityTime;
    private Action<Vine, float> _sendVine;

    private float _currentVineHealth;
    private float _vineHealth;

    private void OnEnable()
    {
        BossManager.Instance.RegisetVine(this);
    }

    void Start()
    {
        InitializeVine();
    }    

    private void InitializeVine()
    {       
        _intensityTime = new WaitForSeconds(0.1f);

        GetVineHealthData();        
    }

    private void GetVineHealthData()
    {
        var data = DataManager.Instance.GetData("B101") as ForestMotherData;

        _vineHealth = data.VineHealth;
        
        _currentVineHealth = _vineHealth;
    }

    public void SetMaterial()
    {
        _copyMaterial = Instantiate(_material);

        _skinnedMeshRenderer.material = _copyMaterial;
    }

    public void TakeDamage(float damage)
    {
        _currentVineHealth -= damage;
        
        if(_currentVineHealth >= 0)
        {
            _sendVine?.Invoke(_vineType, _currentVineHealth);

            IntensityChange(2f, 3f);

            if(_currentVineHealth == 0)
            {
                _currentVineHealth = _vineHealth;
            }
        }
    }

    private IEnumerator IntensityChange(float baseValue, float power)
    {
        Color currentColor = _copyMaterial.GetColor("_Color");

        Color intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _copyMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _copyMaterial.SetColor("_Color", currentColor);
    }

    public void AddVineEvent(Action<Vine, float> callBack, bool isRegistered)
    {
        if (isRegistered)
        {
            _sendVine += callBack;
        }
        else
        {
            _sendVine -= callBack;
        }
    }
}
