using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : BehaviourMonster, IDamged
{
    /*
                                             [Selector]

                   [Selector]                                        [Selector]
      
       [Sequence]             [Sequence]                      [CheckPlayer] [Partrol] 
     
  [CanAttack][Attack]     [Rotation][Move]

     */
    [Header("SoulPosition")]
    [SerializeField] private Transform _soulPosition;
    [Header("AttackObject")]
    [SerializeField] private GameObject[] _objectArray;
    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderer;
    [Header("Material")]
    [SerializeField] private Material _originalMaterial;

    private INode _node;
    private bool _isDead;

    #region Property
    public GameObject PlayerObject { get; set; }
    public List<Transform> PatrolList { get { return _patrolList; } }
    public float SlimeSpeed { get { return _currentSpeed; } }
    public bool CheckPlayer { get; set; }
    public bool CanRotation { get; set; } = true;
    public bool CanMove { get; set; } = true;
    public bool IsAttack { get; set; } = false;
    #endregion

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadMonsterData("M102"));
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

        for(int i = 0; i < _skinnedMeshRenderer.Length; i++)
        {
            _skinnedMeshRenderer[i].material = _copyMaterial;
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
                    new SlimeCanAttack(this),
                    new SlimeAttack(this)
                }),
                new SequenceNode(new List<INode>
                {
                    new SlimeCanRotation(this),
                    new SlimeMoveToPlayer(this)
                })
            }),
            new SelectorNode(new List<INode>
            {
                new SlimeCheckPlayer(this),
                new SlimePatrol(this)
            })
        });
        return node;
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        SkillManager.Instance.SkillCount++;

        if(_currentHp <= 0)
        {
            _isDead = true;

            Die(_soulPosition);
        }
        else
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }

    public void OnUpdateAttackObject()
    {
        float rotation = 0f;

        for (int i = 0; i < _objectArray.Length; i++)
        {
            rotation += 45f;

            SlimeObjectMove objectComponent = _objectArray[i].GetComponent<SlimeObjectMove>();

            if(objectComponent != null)
            {
                objectComponent.SetRotationValue(rotation);

                objectComponent.SetDamage(_currentPower);
            }

            _objectArray[i].SetActive(true);
        }
    }
}
