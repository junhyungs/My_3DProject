//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.UIElements;

//public class MageView : Monster, IDisableMagicBullet
//{
//    [Header("SoulPosition")]
//    [SerializeField] private GameObject _dropSoulPosition;

//    [Header("FirePosition")]
//    [SerializeField] private GameObject _firePosition;

//    private Action _onDisableBulletHandler;
//    private LayerMask _playerLayer;
//    private bool check = false;
//    private bool isDead = false;
//    private bool isTeleporting = true;
//    private int _attackCount = 0;
//    private bool _isAttak;
//    private bool _canMove = false;
//    private float _radius = 10f;
//    private float _teleportDistance = 10f;

//    private void OnEnable()
//    {
//        EventManager.Instance.RegisterDisableMageBullet(this);
//    }

//    protected override void Start()
//    {
//        base.Start();
//        InitializeMage();
//        InitializeMaterial();
//    }

//    private void InitializeMage()
//    {
//        //_data = MonsterManager.Instance.GetMonsterData(MonsterType.Mage);

//        //m_monsterHealth = _data._health;
//        //m_monsterAttackPower = _data._attackPower;
//        //m_monsterSpeed = _data._speed;
//        //m_monsterAgent.speed = m_monsterSpeed;
//        //_playerLayer = LayerMask.GetMask("Player");
//        //_node = new EnemyNode(SetUpTree());
//    }

//    private void InitializeMaterial()
//    {
//        m_copyMaterial = Instantiate(m_originalMaterial);
//        m_skinnedMeshRenderer.material = m_copyMaterial;
//        m_saveColor = m_copyMaterial.GetColor("_Color");
//    }

//    private void Die()
//    {
//        if (isSpawn)
//        {
//            GimikManager.Instance.UnRegisterMonster(this.gameObject);

//            isSpawn = false;
//        }

//        GameObject soul = PoolManager.Instance.GetSoul();
//        DropSoul soulComponent = soul.GetComponent<DropSoul>();
//        soul.transform.SetParent(_dropSoulPosition.transform);
//        soul.transform.localPosition = Vector3.zero;
//        soul.SetActive(true);
//        soulComponent.StartCoroutine(soulComponent.Fly());
//        soul.transform.parent = null;

//        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
//        m_monsterAgent.SetDestination(transform.position);
//        m_monsterAgent.isStopped = true;
//        m_monsterRigid.isKinematic = true;
//        m_monsterAnim.SetTrigger("Die");
//        _onDisableBulletHandler?.Invoke();
//        StartCoroutine(Die(5f, 0.5f, 0.003f));
//    }

//    public void IsSpawn(bool isSpawn)
//    {
//        this.isSpawn = isSpawn;
//    }

//    public override void TakeDamage(float damage)
//    {
//        m_monsterHealth -= (int)damage;

//        SkillManager.Instance.SkillCount++;

//        if(m_monsterHealth <= 0)
//        {
//            isDead = true;

//            Die();
//        }
//        else
//        {
//            StartCoroutine(IntensityChange(2f, 3f));
//        }
//    }

//    public void OnDisableMagicBullet(Action callBack)
//    {
//        _onDisableBulletHandler += callBack;
//    }

//    public void MagicBullet()
//    {
//        GameObject magicBullet = PoolManager.Instance.GetMagicBullet();

//        MagicBullet magicBulletComponent = magicBullet.GetComponent<MagicBullet>();
//        magicBulletComponent.IsFire(false);
//        magicBulletComponent.SetAttackPower(m_monsterAttackPower);
//        magicBullet.transform.SetParent(_firePosition.transform);
//        magicBullet.transform.localPosition = Vector3.zero;
//        magicBullet.transform.localRotation = _firePosition.transform.localRotation;
//        GameObject particle = magicBullet.transform.GetChild(0).gameObject;
//        particle.SetActive(true);
//    }

//    public void MagicBulletfire()
//    {
//        if (_firePosition.transform.childCount != 0)
//        {
//            GameObject magicBullet = _firePosition.transform.GetChild(0).gameObject;
//            MagicBullet magicBulletComponent = magicBullet.GetComponent<MagicBullet>();
//            magicBulletComponent.IsFire(true);
//            magicBullet.transform.parent = null;
//        }
//    }

//    public void OnTelePort()
//    {
//        _isAttak = false;
//    }

//    void Update()
//    {
//        if (!isDead)
//        {
//            _node.Execute();
//        }
//    }


//    private INode SetUpTree()
//    {
//        var canTeleportList = new List<INode>();
//        canTeleportList.Add(new EnemyAction(CanTelePort));
//        canTeleportList.Add(new EnemyAction(TelePort));
//        var canTelePortSequence = new EnemySequence(canTeleportList);

//        var actionList = new List<INode>();
//        actionList.Add(new EnemyAction(MoveToPlayer));
//        actionList.Add(new EnemyAction(RotateToPlayer));
//        actionList.Add(new EnemyAction(Attack));
//        var attackSequence = new EnemySequence(actionList);
   
//        var telePortAndAttackList = new List<INode>();
//        telePortAndAttackList.Add(canTelePortSequence);
//        telePortAndAttackList.Add(attackSequence);
//        var actionSelectorNode = new EnemySelector(telePortAndAttackList);

//        var actionAndcheckPlayerList = new List<INode>();
//        actionAndcheckPlayerList.Add(actionSelectorNode);
//        actionAndcheckPlayerList.Add(new EnemyAction(CheckPlayer));

//        var mageNode = new EnemySelector(actionAndcheckPlayerList);

//        return mageNode;
//    }

//    private INode.State CheckPlayer()
//    {
//        if (check)
//            return INode.State.Success;

//        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _playerLayer);

//        if (colliders.Length > 0)
//        {
//            check = true;
//            return INode.State.Fail;
//        }

//        return INode.State.Running;
//    }

//    private INode.State RotateToPlayer()
//    {
//        if(_attackCount > 0)
//        {
//            return INode.State.Success;
//        }

//        Vector3 targetDirection = (m_player.transform.position - transform.position).normalized;

//        targetDirection.y = 0f;

//        Quaternion rotation = Quaternion.LookRotation(targetDirection);

//        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 50f * Time.deltaTime);

//        LayerMask targetLayer = LayerMask.GetMask("Player");

//        RaycastHit[] hit = Physics.RaycastAll(transform.position, transform.forward, 20f, targetLayer);

//        if(hit.Length > 0)
//        {
//            return INode.State.Success;
//        }

//        return INode.State.Running;
//    }

//    private INode.State Attack()
//    {
//        if(_attackCount > 0)        
//        {
            
//            if (!_isAttak)
//            {
//                isTeleporting = true;

//                _attackCount = 0;

//                return INode.State.Success;
//            }

//            return INode.State.Fail;
//        }

//        m_monsterAnim.SetTrigger("Attack");

//        _isAttak = true;

//        _attackCount = 1;

//        return INode.State.Success;
//    }

//    private INode.State MoveToPlayer()
//    {
//        if(!check)
//            return INode.State.Fail;

//        if (!_canMove)
//        {
//            return INode.State.Fail;
//        }

//        float currentDistance = Vector3.Distance(transform.position, m_player.transform.position);

//        if(currentDistance > m_monsterAgent.stoppingDistance)
//        {
//            m_monsterAgent.SetDestination(m_player.transform.position);
//        }

//        return INode.State.Success;
//    }

//    private INode.State CanTelePort()
//    {
//        if (!check)
//        {
//            return INode.State.Fail;
//        }

//        if (isTeleporting)
//        {
//            isTeleporting = false;

//            return INode.State.Success;
//        }

//        return INode.State.Fail;
//    }

//    private INode.State TelePort()
//    {
//        StartCoroutine(TelePortCoroutine());

//        return INode.State.Success;
//    }

//    private IEnumerator TelePortCoroutine()
//    {
//        yield return StartCoroutine(TelePort_In());

//        yield return StartCoroutine(TelePort_Out());
//    }


//    private IEnumerator TelePort_In()
//    {
//        _canMove = false;

//        m_monsterAnim.SetTrigger("TelePort");

//        float timer = 2.5f;

//        float colorAmount = 0.005f;

//        float colorMaxValue = 0.5f;

//        while(timer > 0f)
//        {
//            colorMaxValue -= colorAmount;
//            m_copyMaterial.SetFloat("_Float", colorMaxValue);
//            yield return null;
//            timer -= Time.deltaTime;
//        }
//    }
    
//    private IEnumerator TelePort_Out()
//    {
//        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _teleportDistance;

//        randomDirection.y = 0f;

//        Vector3 telePortPosition = m_player.transform.position + randomDirection;

//        NavMeshHit hit;

//        if(NavMesh.SamplePosition(telePortPosition, out hit, _teleportDistance, NavMesh.AllAreas))
//        {
//            m_monsterAgent.Warp(hit.position);

//            Vector3 rotateToPlayer = (m_player.transform.position - transform.position).normalized;

//            rotateToPlayer.y = 0f;

//            if(rotateToPlayer != Vector3.zero)
//            {
//                transform.rotation = Quaternion.LookRotation(rotateToPlayer);
//            }
//        }

//        yield return new WaitForSeconds(0.5f);

//        m_monsterAnim.SetBool("TelePort_In", true);

//        float timer = 2.5f;

//        float colorAmount = 0.005f;

//        float colorMaxValue = -0.3f;

//        while (timer > 0f)
//        {
//            colorMaxValue += colorAmount;
//            m_copyMaterial.SetFloat("_Float", colorMaxValue);
//            yield return null;
//            timer -= Time.deltaTime;
//        }

//        m_monsterAnim.SetBool("TelePort_In", false);

//        _canMove = true;
//    }


//}
