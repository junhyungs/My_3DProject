using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    
    #region Property
    public GameObject PlayerObject { get; set; }
    public BT_MonsterData Data => _data;
    public bool Spawn => _isSpawn;
    public bool CheckPlayer { get; set; }
    public bool CanRotation { get; set; } = true;
    public bool CanMove { get; set; } = true;
    public bool IsAttack { get; set; } = false;
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadMonsterData("M102"));
        SetMaterial();

        _monsterType = ObjectName.Slime;
    }

    private void Update()
    {
        if (!_isDead && _dataReady)
        {
            _behaviourNode.Evaluate();
        }
    }

    public override void IsSpawn(bool isSpawn, SpawnMonster reference)
    {
        base.IsSpawn(isSpawn, reference);
    }

    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_originalMaterial);

        for(int i = 0; i < _skinnedMeshRenderer.Length; i++)
        {
            _skinnedMeshRenderer[i].material = _copyMaterial;
        }
    }

    protected override INode SetBehaviourTree()
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

    private Vector3 _gridCenter;
    private Vector3 _spherePosition;
    private float _radius;
    private float _size;
    public void SetGrid(Vector3 gridCenter, float size)
    {
        _gridCenter = gridCenter;
        _size = size;
    }

    public void SetSphere(Vector3 spherePosition, float radius)
    {
        _spherePosition = spherePosition;
        _radius = radius;
    }

    private void OnDrawGizmos()
    {
        if(_gridCenter != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_gridCenter, new Vector3(_size, transform.position.y, _size));
        }

        if( _spherePosition != Vector3.zero)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_spherePosition, _radius);
        }
    }


}
