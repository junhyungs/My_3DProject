using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MageBehaviour : BehaviourMonster, IDamged, IDisableMagicBullet
{
    private INode _node;
    private bool _isDead;
    private Action _disableHandler;

    #region Property
    public Material CopyMaterial { get { return _copyMaterial; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public Animator Animator { get { return _animator; } }
    public GameObject PlayerObject {  get; set; }
    public bool CheckPlayer { get; set; }
    public bool IsAttack {  get; set; }
    public bool IsTeleporting { get; set; } = true;
    public bool CanMove {  get; set; }
    public int AttackCount { get; set; }
    public Coroutine TelePort { get; set; }
    #endregion

    [Header("SoulPosition")]
    [SerializeField] private Transform _soulPosition;
    [Header("FirePosition")]
    [SerializeField] private Transform _firePosition;
    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [Header("Material")]
    [SerializeField] private Material _originalMaterial;

    private void OnEnable()
    {
        EventManager.Instance.RegisterDisableMageBullet(this);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadMonsterData("M103"));
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

    private INode SetBehaviourTree()
    {
        INode node = new SelectorNode(new List<INode>
        {
            new SelectorNode(new List<INode>
            {
                new SequenceNode(new List<INode>
                {
                    new MageCanTelePort(this),
                    new MageTelePort(this)
                }),
                new SequenceNode(new List<INode>
                {
                    new MageMoveToPlayer(this),
                    new MageRotateToPlayer(this),
                    new MageAttack(this)
                })
            }),
            new MageCheckPlayer(this)
        });

        return node;
    }

    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_originalMaterial);
        _skinnedMeshRenderer.material = _copyMaterial;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= (int)damage;

        SkillManager.Instance.AddSkillCount();

        if(_currentHp <= 0)
        {
            _isDead = true;

            if(TelePort != null)
            {
                StopCoroutine(TelePort);

                TelePort = null;
            }

            Die(_soulPosition, _disableHandler);
        }
        else
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }

    public void MagicBullet()
    {
        GameObject magicBullet = PoolManager.Instance.GetMagicBullet();

        MagicBullet component = magicBullet.GetComponent<MagicBullet>();
        component.IsFire(false);
        component.SetAttackPower(_currentPower);
        magicBullet.transform.SetParent(_firePosition);
        magicBullet.transform.localPosition = Vector3.zero;
        magicBullet.transform.localRotation = _firePosition.transform.localRotation;
        GameObject particle = magicBullet.transform.GetChild(0).gameObject;
        particle.SetActive(true);
    }

    public void MagicBulletfire()
    {
        if(_firePosition.transform.childCount != 0)
        {
            GameObject magicBullet = _firePosition.transform.GetChild(0).gameObject;
            MagicBullet component = magicBullet.GetComponent<MagicBullet>();
            component.IsFire(true);
            magicBullet.transform.parent = null;
        }
    }

    public void OnTelePort()
    {
        IsAttack = false;
    }

    public void OnDisableMagicBullet(Action callBack)
    {
        _disableHandler += callBack;
    }
}
