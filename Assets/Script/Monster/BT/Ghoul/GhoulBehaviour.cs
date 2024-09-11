using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulBehaviour : BehaviourMonster, IDamged, IDisableArrow
{
    private INode _node;
    private Action _disableHandler;

    [Header("SoulPosition")]
    [SerializeField] private Transform _soulTransform;
    [Header("FirePosition")]
    [SerializeField] private Transform _fireTransform;
    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [Header("Material")]
    [SerializeField] private Material _originalMaterial;
    [Header("BowMeshRenderer")]
    [SerializeField] private MeshRenderer _meshRenderer;

    private void OnEnable()
    {
        EventManager.Instance.RegisterDisableGhoulArrow(this);
    }

    protected override void Start()
    {
        base.Start();
        SetData(MonsterType.Ghoul);
        SetMaterial();
    }

    private void Update()
    {
        
    }

    private INode SetBehaviourTree()
    {
        return _node;
    }

    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_originalMaterial);
        _skinnedMeshRenderer.material = _copyMaterial;
        _meshRenderer.material = _copyMaterial;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        if(_currentHp <= 0)
        {
            Die(_soulTransform, _disableHandler);
        }
        else
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }

    public void OnDisableArrow(Action callBack)
    {
        _disableHandler += callBack;
    }

    public void Arrow()
    {
        GameObject arrow = PoolManager.Instance.GetMonsterArrow();

        GhoulArrow arrowComponent = arrow.GetComponent<GhoulArrow>();
        arrowComponent.IsFire(false);
        arrowComponent.SetAttackPower(_currentPower);
        arrow.transform.SetParent(_fireTransform);
        arrow.transform.localPosition = Vector3.zero;
        arrow.transform.rotation = _fireTransform.rotation;
    }

    public void FireArrow()
    {
        if(_fireTransform.childCount != 0)
        {
            GameObject arrow = _fireTransform.GetChild(0).gameObject;
            GhoulArrow arrowComponent = arrow.GetComponent<GhoulArrow>();
            arrowComponent.IsFire(true);
            arrow.transform.parent = null;
        }
    }
}
