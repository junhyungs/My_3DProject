using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulBehaviour : BehaviourMonster, IDamged, IDisableArrow
{
    private INode _node;
    private bool _isDead;
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

    public NavMeshAgent Agent { get { return _agent; } }
    public Animator Animator { get { return _animator; } }
    public GameObject PlayerObject { get; set; }
    public bool CheckPlayer { get; set; }
    public bool CanMove { get; set; } = true;
    public bool CanRotation { get; set; } = true;
    public bool CanAttack { get; set; } = true;


    private void Awake()
    {
        EventManager.Instance.RegisterDisableGhoulArrow(this);
    }

    protected override void Start()
    {
        base.Start();
        SetData(MonsterType.Ghoul);
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

    private INode SetBehaviourTree()
    {
        INode node = new SelectorNode(new List<INode>
        {
            new SequenceNode(new List<INode>
            {
                new GhoulMoveToPlayer(this),
                new GhoulRotateToPlayer(this),
                new GhoulAttack(this)
            }),

            new GhoulCheckPlayer(this)
        });

        return node;
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
            _isDead = true;

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