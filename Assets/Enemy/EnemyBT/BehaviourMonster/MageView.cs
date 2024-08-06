using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MageView : Monster, IDisableMagicBullet
{
    [Header("SoulPosition")]
    [SerializeField] private GameObject _dropSoulPosition;

    [Header("FirePosition")]
    [SerializeField] private GameObject _firePosition;

    private Action _onDisableBulletHandler;
    private LayerMask _playerLayer;
    private INode.State _teleportState = INode.State.Running;
    private bool check = false;
    private bool isDead = false;
    private bool isTeleporting = false;
    private bool canTeleport = true;

    private bool _canAttack = false;

    private float _radius = 10f;
    private float _teleportDistance = 10f;

    private void OnEnable()
    {
        EventManager.Instance.RegisterDisableMageBullet(this);
    }

    protected override void Start()
    {
        base.Start();
        InitializeMage();
        InitializeMaterial();
    }

    private void InitializeMage()
    {
        _data = MonsterManager.Instance.GetMonsterData(MonsterType.Mage);

        m_monsterHealth = _data._health;
        m_monsterAttackPower = _data._attackPower;
        m_monsterSpeed = _data._speed;
        m_monsterAgent.speed = m_monsterSpeed;
        _playerLayer = LayerMask.GetMask("Player");
        _node = new EnemyNode(SetUpTree());
    }

    private void InitializeMaterial()
    {
        m_copyMaterial = Instantiate(m_originalMaterial);
        m_skinnedMeshRenderer.material = m_copyMaterial;
        m_saveColor = m_copyMaterial.GetColor("_Color");
    }

    private void Die()
    {
        if (isSpawn)
        {
            GimikManager.Instance.UnRegisterMonster(this.gameObject);

            isSpawn = false;
        }

        GameObject soul = PoolManager.Instance.GetSoul();
        DropSoul soulComponent = soul.GetComponent<DropSoul>();
        soul.transform.SetParent(_dropSoulPosition.transform);
        soul.transform.localPosition = Vector3.zero;
        soul.SetActive(true);
        soulComponent.StartCoroutine(soulComponent.Fly());
        soul.transform.parent = null;

        gameObject.layer = LayerMask.NameToLayer("DeadMonster");
        m_monsterAgent.SetDestination(transform.position);
        m_monsterAgent.isStopped = true;
        m_monsterRigid.isKinematic = true;
        m_monsterAnim.SetTrigger("Die");
        _onDisableBulletHandler?.Invoke();
        StartCoroutine(Die(5f, 0.5f, 0.003f));
    }

    public void IsSpawn(bool isSpawn)
    {
        this.isSpawn = isSpawn;
    }

    public override void TakeDamage(float damage)
    {
        m_monsterHealth -= (int)damage;

        SkillManager.Instance.AddSkillCount();

        if(m_monsterHealth <= 0)
        {
            isDead = true;

            Die();
        }
        else
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }

    public void OnDisableMagicBullet(Action callBack)
    {
        _onDisableBulletHandler += callBack;
    }

    public void MagicBullet()
    {
        GameObject magicBullet = PoolManager.Instance.GetMagicBullet();

        MagicBullet magicBulletComponent = magicBullet.GetComponent<MagicBullet>();
        magicBulletComponent.IsFire(false);
        magicBulletComponent.SetAttackPower(m_monsterAttackPower);
        magicBullet.transform.SetParent(_firePosition.transform);
        magicBullet.transform.localPosition = Vector3.zero;
        magicBullet.transform.localRotation = _firePosition.transform.localRotation;
        GameObject particle = magicBullet.transform.GetChild(0).gameObject;
        particle.SetActive(true);
    }

    public void MagicBulletfire()
    {
        if (_firePosition.transform.childCount != 0)
        {
            GameObject magicBullet = _firePosition.transform.GetChild(0).gameObject;
            MagicBullet magicBulletComponent = magicBullet.GetComponent<MagicBullet>();
            magicBulletComponent.IsFire(true);
            magicBullet.transform.parent = null;
        }
    }

    void Update()
    {
        if (!isDead)
        {
            _node.Execute();

            var rr = m_monsterAnim.GetCurrentAnimatorStateInfo(0);
            Debug.Log(rr.IsName("ShootBT"));
        }
    }


    private INode SetUpTree()
    {
        var actionList = new List<INode>();
        actionList.Add(new EnemyAction(TelePort));
        actionList.Add(new EnemyAction(RotateToPlayer));
        actionList.Add(new EnemyAction(Attack));

        var attackSequence = new EnemySequence(actionList);

        var moveList = new List<INode>();
        moveList.Add(new EnemyAction(MoveToPlayer));

        var moveSequence = new EnemySelector(moveList);

        var moveAndattackList = new List<INode>();
        moveAndattackList.Add(attackSequence);
        moveAndattackList.Add(moveSequence);

        var moveAndattackSelector = new EnemySelector(moveAndattackList);

        var selectorList = new List<INode>();
        selectorList.Add(moveAndattackSelector);
        selectorList.Add(new EnemyAction(CheckPlayer));

        var selectorNode = new EnemySelector(selectorList);

        return selectorNode;
    }

    private INode.State CheckPlayer()
    {
        if (check)
            return INode.State.Success;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _playerLayer);

        if (colliders.Length > 0)
        {
            check = true;
            return INode.State.Success;
        }

        return INode.State.Running;
    }

    private INode.State RotateToPlayer()
    {
        if (!check)
            return INode.State.Fail;

        Vector3 targetDirection = (m_player.transform.position -transform.position).normalized;

        Vector3 mageForward = transform.forward;

        float angle = Vector3.SignedAngle(mageForward, targetDirection, Vector3.up);

        if (Mathf.Abs(angle) < 5f)
            return INode.State.Success;
        else
        {
            targetDirection.y = 0f;

            Quaternion rotation = Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);

            return INode.State.Running;
        }
            
    }

    private INode.State Attack()
    {
        if (!check)
            return INode.State.Fail;

        m_monsterAnim.SetTrigger("Attack");

        return INode.State.Fail;
    }

    private INode.State MoveToPlayer()
    {
        if(!check)
            return INode.State.Fail;

        var animatorStateInfo = m_monsterAnim.GetCurrentAnimatorStateInfo(0);
       
        if (animatorStateInfo.IsName("ShootBT") && animatorStateInfo.normalizedTime < 1.0f)
        {
            float distance = Vector3.Distance(transform.position, m_player.transform.position);

            if (distance >= 8f)
            {
                m_monsterAgent.SetDestination(m_player.transform.position);
            }

            return INode.State.Running;
        }    
        
        isTeleporting = false;

        return INode.State.Success;
    }

    private INode.State TelePort()
    {
        if(!check)
            return INode.State.Fail;

        if (isTeleporting)
            return _teleportState;

        var animatorStateInfo = m_monsterAnim.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("ShootBT") && animatorStateInfo.normalizedTime < 1.0f)
        {
            return INode.State.Fail;
        }
        else
        {
            StartCoroutine(TelePortCoroutine());

            return INode.State.Running;
        }
    }

    private IEnumerator TelePortCoroutine()
    {
        yield return StartCoroutine(TelePort_In());

        yield return StartCoroutine(TelePort_Out());
    }


    private IEnumerator TelePort_In()
    {
        _teleportState = INode.State.Running;

        isTeleporting = true;

        m_monsterAnim.SetTrigger("TelePort");

        float timer = 2.5f;

        float colorAmount = 0.005f;

        float colorMaxValue = 0.5f;

        while(timer > 0f)
        {
            colorMaxValue -= colorAmount;
            m_copyMaterial.SetFloat("_Float", colorMaxValue);
            yield return null;
            timer -= Time.deltaTime;
        }
    }
    
    private IEnumerator TelePort_Out()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _teleportDistance;

        randomDirection.y = 0f;

        Vector3 telePortPosition = m_player.transform.position + randomDirection;

        NavMeshHit hit;

        if(NavMesh.SamplePosition(telePortPosition, out hit, _teleportDistance, NavMesh.AllAreas))
        {
            m_monsterAgent.Warp(hit.position);

            Vector3 rotateToPlayer = m_player.transform.position - transform.position;

            rotateToPlayer.y = 0f;

            if(rotateToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(rotateToPlayer);
            }
        }

        yield return new WaitForSeconds(0.5f);

        m_monsterAnim.SetBool("TelePort_In", true);

        float timer = 2.5f;

        float colorAmount = 0.005f;

        float colorMaxValue = -0.3f;

        while (timer > 0f)
        {
            colorMaxValue += colorAmount;
            m_copyMaterial.SetFloat("_Float", colorMaxValue);
            yield return null;
            timer -= Time.deltaTime;
        }

        m_monsterAnim.SetBool("TelePort_In", false);

        _teleportState = INode.State.Success;
    }


}
