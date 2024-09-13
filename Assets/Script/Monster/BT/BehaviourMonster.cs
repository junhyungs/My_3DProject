using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourMonster : MonoBehaviour
{
    #region Component
    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected Rigidbody _rigidBody;
    protected Material _copyMaterial;
    #endregion

    #region Value
    protected float _currentHp;
    protected float _currentPower;
    protected float _currentSpeed;
    protected bool _isSpawn;
    protected Color _saveColor;
    private WaitForSeconds _intensityTime = new WaitForSeconds(0.1f);

    [Header("PartrolPoint")]
    [SerializeField] protected List<Transform> _patrolList;
    #endregion

    #region Data
    protected MonsterData _data;
    #endregion

    protected virtual void Start()
    {
        InitializeComponent();
    }

    protected void InitializeComponent()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _animator = gameObject.GetComponent<Animator>();
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    protected void SetData(MonsterType monsterType)
    {
        _data = MonsterManager.Instance.GetMonsterData(monsterType);

        _currentHp = _data._health;
        _currentPower = _data._attackPower;
        _currentSpeed = _data._speed;

        if(_agent != null)
        {
            _agent.speed = _currentSpeed;
        }
        
    }

    protected IEnumerator IntensityChange(float baseValue, float power)
    {
        Color currentColor = _copyMaterial.GetColor("_Color");

        Color intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _copyMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _copyMaterial.SetColor("_Color", currentColor);
    }

    protected void Die(Transform soulTransform, Action objectEvent = null)
    {
        if (_isSpawn)
        {
            GimikManager.Instance.UnRegisterMonster(this.gameObject);

            _isSpawn = false;
        }

        MonsterSoul(soulTransform);

        this.gameObject.layer = LayerMask.NameToLayer("DeadMonster");

        objectEvent?.Invoke();

        _animator.SetTrigger("Die");

        StartCoroutine(FireShader(5f, 0.5f, -0.3f));
    }

    protected IEnumerator FireShader(float maxTime, float startValue, float endValue)
    {
        float elapsedTime = 0f;

        while(elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;

            float colorValue = Mathf.Lerp(startValue, endValue, elapsedTime / maxTime);

            _copyMaterial.SetFloat("_Float", colorValue);

            yield return null;
        }

        this.gameObject.SetActive(false);
    }

    protected void MonsterSoul(Transform soulTransform)
    {
        GameObject soul = PoolManager.Instance.GetSoul();
        DropSoul component = soul.GetComponent<DropSoul>();
        soul.transform.position = soulTransform.position;
        soul.SetActive(true);
        component.StartCoroutine(component.Fly());
    }

    public void IsSpawn(bool isSpawn)
    {
        _isSpawn = isSpawn;
    }
}
