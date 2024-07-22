using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class OldCrow : MonoBehaviour
{
    #region Component
    private BossStateMachine _state;
    private GameObject _player;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    private BossData _data;
    private IPattern _currentPattern;
    private BossPattern _patternType;
    #endregion

    #region Value
    private int _id;
    private string _name;
    private float _speed;
    private float _hp;
    private float _power;
    #endregion

    #region Property

    #endregion

    private void Awake()
    {
        InitializeComponent();
        InitializeState();
    }

    private void Start()
    {
        InitializeOldCrow();
    }

    private void InitializeComponent()
    {
        _state = gameObject.AddComponent<BossStateMachine>();
        _player = GameManager.Instance.Player;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void InitializeOldCrow()
    {
        _data = BossManager.Instance.GetBossData(BossType.OldCrow);
        _id = _data._id;
        _name = _data._name;
        _speed = _data._speed;
        _hp = _data._hp;
        _power = _data._power;
    }

    private void InitializeState()
    {
        _state.AddState(BossState.Dash, new OldCrow_Dash(this));
        _state.AddState(BossState.Jump, new OldCrow_Jump(this));
        _state.AddState(BossState.MegaDash, new  OldCrow_MegaDash(this));
        _state.AddState(BossState.Egg, new  OldCrow_Egg(this));
        _state.AddState(BossState.Scream , new OldCrow_Scream(this));   
    }

    public void SetPattern(BossPattern pattern)
    {
        _patternType = pattern;

        Component component = gameObject.GetComponent<IPattern>() as Component;

        if (component != null)
        {
            Destroy(component);
        }

        switch (pattern)
        {
            case BossPattern.MegaDash:
                _currentPattern = new MegaDash(this);
                break;
            case BossPattern.Egg:
                _currentPattern = new Egg(this);
                break;
            case BossPattern.Scream:
                _currentPattern = new Scream(this);
                break;
        }
    }
}
