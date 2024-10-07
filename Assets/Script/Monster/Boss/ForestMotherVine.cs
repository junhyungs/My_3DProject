using System.Collections;
using UnityEngine;
using System;

public enum Vine
{
    Left,
    Right,
    Null
}

public class ForestMotherVine : MonoBehaviour, IDamged, ISendVineEvent
{
    [Header("VineType")]
    [SerializeField] private Vine _vineType;
    [Header("VineSkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

    private WaitForSeconds _intensityTime;
    private Action<Vine, float> _sendVine;
    private CapsuleCollider _vineCollider;
    private Material _saveMaterial;
    private Material _newMaterial;

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
        _vineCollider = GetComponent<CapsuleCollider>();
        _newMaterial = new Material(_skinnedMeshRenderer.sharedMaterial);

        GetVineHealthData();
    }

    private void GetVineHealthData()
    {
        var data = DataManager.Instance.GetData("B101") as ForestMotherData;

        _vineHealth = data.VineHealth;
        
        _currentVineHealth = _vineHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentVineHealth -= damage;
        
        if(_currentVineHealth >= 0)
        {
            _sendVine?.Invoke(_vineType, _currentVineHealth);

            StartCoroutine(IntensityChange(2f, 3f));

            if (_currentVineHealth == 0)
            {
                _currentVineHealth = _vineHealth;
            }
        }
    }

    public void SaveMaterial()
    {
        _saveMaterial = _skinnedMeshRenderer.sharedMaterial;
    }

    public void EnabledCollider(bool enabled)
    {
        _vineCollider.enabled = enabled;
    }

    private IEnumerator IntensityChange(float baseValue, float power)
    {
        _skinnedMeshRenderer.material = _newMaterial;

        Color currentColor = _newMaterial.GetColor("_Color");
        
        Color intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _newMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _newMaterial.SetColor("_Color", currentColor);

        _skinnedMeshRenderer.material = _saveMaterial;
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
