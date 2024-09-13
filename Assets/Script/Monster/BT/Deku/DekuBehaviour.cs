using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuBehaviour : BehaviourMonster, IDamged
{
    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [Header("Material")]
    [SerializeField] private Material _originalMaterial;
    [Header("SoulTransform")]
    [SerializeField] private Transform _soulTransform;
    [Header("FireTransform")]
    [SerializeField] private Transform _fireTransform;

    private INode _node;
    private bool _isDead;

    protected override void Start()
    {
        base.Start();
        SetData(MonsterType.Deku);
        SetMaterial();

        _node = SetBehaviourTree();
    }

    private void Update()
    {
        if (!_isDead)
        {
            _node.Evaluate();
        }
    }

    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_originalMaterial);
        _skinnedMeshRenderer.material = _copyMaterial;
    }
    
    private INode SetBehaviourTree()
    {
        return _node;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        if(_currentHp <= 0)
        {
            _isDead = true;

            Die(_soulTransform);
        }
        else
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }

    public void DekuProjectile()
    {
        GameObject bullet = PoolManager.Instance.GetDekuProjectile();

        bullet.transform.position = _fireTransform.position;
        bullet.transform.rotation = _fireTransform.rotation;
        DekuBullet bulletComponent = bullet.GetComponent<DekuBullet>();
        bulletComponent.IsFire(true);
    }
}
