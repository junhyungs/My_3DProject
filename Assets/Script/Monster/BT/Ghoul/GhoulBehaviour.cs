using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhoulBehaviour : BehaviourMonster, IDamged, IDisableArrow
{
  

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

    public GameObject PlayerObject { get; set; }
    public BT_MonsterData Data => _data;
    public Vector3 Destination { get; set; }
    public bool Spawn => _isSpawn;
    public bool CheckPlayer { get; set; }
    public bool IsAttack { get; set; } = false;
    public bool IsReturn { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisableMonster()
    {
        base.OnDisableMonster();
    }

    protected override void Start()
    {
        ObjectPool.Instance.CreatePool(ObjectName.GhoulArrow, 50);
        base.Start();
        StartCoroutine(LoadMonsterData("M106"));
        SetMaterial();

        _monsterType = ObjectName.Ghoul;
    }

    private void Update()
    {
        if (!_isDead && _dataReady)
        {
            _behaviourNode.Evaluate();
        }
    }

    protected override INode SetBehaviourTree()
    {
        
        INode node = new SelectorNode(new List<INode>
        {
            new SequenceNode(new List<INode>()
            {
                new GhoulCanAttack(this),
                new GhoulRotateToPlayer(this),
                new GhoulAttack(this)
            }),

            new SequenceNode(new List<INode>()
            {
                new GhoulIsMove(this),
                new GhoulMoveToPlayer(this),
            }),

            new SelectorNode(new List<INode>
            {
                new GhoulCheckPlayer(this),
                new GhoulPatrol(this),
            })
        });

        return node;
    }

    public override void IsSpawn(bool isSpawn, SpawnMonster reference)
    {
        base.IsSpawn(isSpawn, reference);
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
        GameObject arrow = ObjectPool.Instance.DequeueObject(ObjectName.GhoulArrow);
        GhoulArrow arrowComponent = arrow.GetComponent<GhoulArrow>();

        _disableHandler = arrowComponent.ReturnArrow;
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
    private Vector3 _boundsCenter;
    private Vector3 _boundsSize;

    private Vector3 _girdCenter;
    private float _gridSize;

    private List<Vector3> testList;
    private Bounds _myBounds;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(_boundsCenter != Vector3.zero)
        {
            Gizmos.DrawWireCube( _boundsCenter, _boundsSize);
        }

        if(_girdCenter != Vector3.zero)
        {
            Vector3 gridSize = new Vector3(_gridSize, transform.position.y, _gridSize);

            Gizmos.DrawWireCube(_girdCenter, gridSize);
        }

        if(testList != null)
        {
            foreach(var position in testList)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawWireSphere(position, 0.5f);
            }
        }
    }

    public void SetBounds(Bounds bounds)
    {
        _boundsCenter = bounds.center;
        _boundsSize = bounds.size;
    }

    public void SetGrid(Vector3 center, float size)
    {
        _girdCenter = center;
        _gridSize = size;
    }

    public void SetList(List<Vector3> list)
    {
        testList = list;
    }

    public void SetMyBounds(Bounds bounds)
    {
        _myBounds = bounds;
    }
}
