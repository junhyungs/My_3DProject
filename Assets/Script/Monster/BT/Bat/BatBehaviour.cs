using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatBehaviour : BehaviourMonster, IDamged
{
    /*
                                                                                        [Selector]

                                                   [Selector]                                                      [CheckPlayer]
                                                                           
                               [Sequence]                               [Move]             

                   [CanAttack] [Rotation]  [Attack]                   
     */
    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [Header("Material")]
    [SerializeField] private Material _originalMaterial;
    [Header("SoulPosition")]
    [SerializeField] private Transform _soulTransform;

    private INode _node;
    private bool _isDead;
    #region Property
    public NavMeshAgent Agent { get { return _agent; } }
    public Animator Animator { get { return _animator; } }
    public Rigidbody Rigidbody { get { return _rigidBody; } }
    public GameObject PlayerObject { get; set; }
    public bool CheckPlayer { get; set; }
    public bool IsAttack { get; set; }
    public float CurrentPower { get { return _currentPower; } }
    public float CurrentHP { get { return _currentHp; } }

    #endregion

    protected override void Start()
    {
        base.Start();
        SetData(MonsterType.Bat);
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
        INode node = new SelectorNode(new List<INode>
        {
            new SelectorNode(new List<INode>
            {
                new SequenceNode(new List<INode>
                {
                    new BatCanAttack(this),
                    new BatRotateToPlayer(this),
                    new BatAttack(this)
                }),
                new BatMoveToPlayer(this)
            }),

            new BatCheckPlayer(this)
        }); ;

        return node;
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
}