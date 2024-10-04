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
    protected bool _dataReady = false;
    protected Color _saveColor;
    private WaitForSeconds _intensityTime = new WaitForSeconds(0.1f);
    #endregion

    #region Data
    protected BT_MonsterData _data;
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

        ObjectPool.Instance.CreatePool(ObjectName.Soul, 20);
    }

    protected IEnumerator LoadMonsterData(string id)
    {
        yield return new WaitWhile(() => 
        {
            Debug.Log("몬스터 데이터를 가져오지 못했습니다.");
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as BT_MonsterData;

        _currentHp = data.Health;
        _currentPower = data.Power;
        _currentSpeed = data.Speed;

        if(_agent != null)
        {
            _agent.speed = _currentSpeed;
        }

        _data = data;
        _dataReady = true;
    }

    public IEnumerator IntensityChange(float baseValue, float power)
    {
        Color currentColor = _copyMaterial.GetColor("_Color");

        Color intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _copyMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _copyMaterial.SetColor("_Color", currentColor);
    }

    public IEnumerator Test(float baseValue, float power)
    {
        Color currentColor = _copyMaterial.GetColor("_Color");

        Color intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _copyMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _copyMaterial.SetColor("_Color", currentColor);
    }

    public void Die(Transform soulTransform, Action objectEvent = null)
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

    public IEnumerator FireShader(float maxTime, float startValue, float endValue)
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

    public void MonsterSoul(Transform soulTransform)
    {
        GameObject soul = ObjectPool.Instance.DequeueObject(ObjectName.Soul);
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
