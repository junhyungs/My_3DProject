using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourMonster : MonoBehaviour
{
    #region Component
    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected Rigidbody _rigidBody;
    protected Material _copyMaterial;
    protected SpawnMonster _spawnComponent;

    protected INode _behaviourNode;
    #endregion

    #region Value
    protected float _currentHp;
    protected float _currentPower;
    protected float _currentSpeed;
    protected bool _isSpawn;
    protected bool _dataReady = false;
    protected bool _isDead;
    protected Color _saveColor;
    protected ObjectName _monsterType;
    private WaitForSeconds _intensityTime = new WaitForSeconds(0.1f);
    #endregion

    #region Data
    protected BT_MonsterData _data;
    #endregion


    protected virtual void OnEnable()
    {
        OnEnableMonster();
    }

    private void OnEnableMonster()
    {
        _isDead = false;

        NavMeshAgentControl(_isDead);

        if (_copyMaterial != null && _data != null)
        {
            _copyMaterial.SetFloat("_Float", 0.5f);

            gameObject.layer = LayerMask.NameToLayer("Monster");

            _currentHp = _data.Health;
        }
    }

    protected virtual void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();

        _animator = gameObject.GetComponent<Animator>();

        _rigidBody = gameObject.GetComponent<Rigidbody>();

        ObjectPool.Instance.CreatePool(ObjectName.Soul, 20);    
    }

    public virtual void OnDisableMonster()
    {
        ObjectPool.Instance.EnqueueObject(gameObject, _monsterType);
    }

    protected IEnumerator LoadMonsterData(string id)
    {
        yield return new WaitWhile(() => 
        {
            return DataManager.Instance.GetData(id) == null;
        });

        Debug.Log("<Monster>데이터를 가져왔습니다.");
        var data = DataManager.Instance.GetData(id) as BT_MonsterData;

        _currentHp = data.Health;
        _currentPower = data.Power;
        _currentSpeed = data.Speed;

        if(_agent != null)
        {
            _agent.speed = _currentSpeed;
        }

        _data = data;

        _behaviourNode = SetBehaviourTree();

        _dataReady = true;
    }

    protected virtual INode SetBehaviourTree()
    {
        INode node = null;

        return node;
    }

    public IEnumerator IntensityChange(float baseValue, float power)
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
            _spawnComponent.UnRegisterMonster(gameObject);

            _isSpawn = false;
        }

        _isDead = true;

        NavMeshAgentControl(_isDead);

        MonsterSoul(soulTransform);

        this.gameObject.layer = LayerMask.NameToLayer("DeadMonster");

        objectEvent?.Invoke();

        _animator.SetTrigger("Die");

        StartCoroutine(FireShader(5f, 0.5f, -0.3f));
    }

    private void NavMeshAgentControl(bool isDead)
    {
        if(_agent == null)
        {
            return;
        }

        _agent.isStopped = isDead;

        if (isDead)
        {
            _agent.velocity = Vector3.zero;

            _agent.ResetPath();
        }
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

    public virtual void IsSpawn(bool isSpawn, SpawnMonster reference)
    {
        _spawnComponent = reference;

        _isSpawn = isSpawn;
    }


}
