using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul_BT : BehaviourMonster, IDamged, IDisableArrow
{
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

    public Action _disableHandler { get; set; }
    public Transform SoulPosition { get { return _soulTransform; } }
    public GameObject PlayerObject { get; set; }
    public float CurrentHealth { get { return _currentHp; } }
    public bool CheckPlayer { get; set; }
    public bool IsAttack { get; set; } = false;
    public bool IsReturn { get; set; }
    public bool IsDead { get; set; }

    protected override void Start()
    {
        ObjectPool.Instance.CreatePool(ObjectName.GhoulArrow, 50);
        base.Start();
        StartCoroutine(LoadMonsterData("M106"));
        SetMaterial();
    }
    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_originalMaterial);
        _skinnedMeshRenderer.material = _copyMaterial;
        _meshRenderer.material = _copyMaterial;
    }

    public void OnDisableArrow(Action callBack)
    {
        _disableHandler += callBack;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        SkillManager.Instance.SkillCount++;

        if( _currentHp > 0 )
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }
    public void Arrow()
    {
        GameObject arrow = ObjectPool.Instance.DequeueObject(ObjectName.GhoulArrow);

        GhoulArrow arrowComponent = arrow.GetComponent<GhoulArrow>();
        arrowComponent.IsFire(false);
        arrowComponent.SetAttackPower(_currentPower);
        arrow.transform.SetParent(_fireTransform);
        arrow.transform.localPosition = Vector3.zero;
        arrow.transform.rotation = _fireTransform.rotation;
    }

    public void FireArrow()
    {
        if (_fireTransform.childCount != 0)
        {
            GameObject arrow = _fireTransform.GetChild(0).gameObject;
            GhoulArrow arrowComponent = arrow.GetComponent<GhoulArrow>();
            arrowComponent.IsFire(true);
            arrow.transform.parent = null;
        }
    }
}
