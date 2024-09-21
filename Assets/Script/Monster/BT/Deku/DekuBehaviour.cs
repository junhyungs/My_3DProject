using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuBehaviour : BehaviourMonster, IDamged
{
    /*
                                                            [Selector] 
                
                                   [Selector]                                     [Selector]
     
                      [Sequence]                [Move]                   [CheckPlayer]     [Hide]

        [CanAttack]  [Rotation]   [Attack]      
     */


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

    #region Property
    public GameObject PlayerObject { get; set; }
    public bool CheckPlayer { get; set; }
    public bool IsAttack { get; set; } = false;
    public bool IsReturn { get; set; } = false;
    #endregion

    protected override void Start()
    {
        ObjectPool.Instance.CreatePool(ObjectName.DekuProjectile, 30);
        base.Start();
        StartCoroutine(LoadMonsterData("M105"));
        SetMaterial();

        _node = SetBehaviourTree();
    }

    private void Update()
    {
        if (!_isDead && _dataReady)
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
        INode newNode = new SelectorNode(new List<INode>
        {
            new SelectorNode(new List<INode>
            {
                new SequenceNode(new List<INode>
                {
                    new DekuCanAttack(this),
                    new DekuRotation(this),
                    new DekuAttack(this)
                }),
                new DekuMove(this)
            }),
            new SelectorNode(new List<INode>
            {
                new DekuCheckPlayer(this),
                new DekuHide(this),
            })
        });

        return newNode;
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
        GameObject bullet = ObjectPool.Instance.DequeueObject(ObjectName.DekuProjectile);

        bullet.transform.position = _fireTransform.position;
        bullet.transform.rotation = _fireTransform.rotation;
        DekuBullet bulletComponent = bullet.GetComponent<DekuBullet>();
        bulletComponent.IsFire(true);
    }
}
