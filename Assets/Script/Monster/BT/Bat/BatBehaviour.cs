using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatBehaviour : BehaviourMonster, IDamged
{
    /*
                                                                                        [Selector]

                                                   [Selector]                                                      [Selector]
                                                                           
                               [Sequence]                               [Move]                            checkPlayer        patrol

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
    public GameObject PlayerObject { get; set; }
    public bool CheckPlayer { get; set; }
    public bool IsAttack { get; set; } = false;
    public bool IsReturn { get; set; } = false;
    #endregion

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadMonsterData("M101"));
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
            new SelectorNode(new List<INode>
            {
                new BatCheckPlayer(this),
                new BatPatrol(this)
            })

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

    private Vector3 _gridCenter;
    private float _gridSize;

    public void SetGrid(Vector3 center, float size)
    {
        _gridCenter = center;
        _gridSize = size;
    } 

    private void OnDrawGizmos()
    {
        if(_gridCenter != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_gridCenter, new Vector3(_gridSize,transform.position.y, _gridSize));
        }
    }
}
