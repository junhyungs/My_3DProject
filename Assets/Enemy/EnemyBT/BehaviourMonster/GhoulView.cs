using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulView : Monster, IDisableArrow
{
    [Header("FirePosition")]
    [SerializeField] private Transform _firePosition;
    [Header("SoulPosition")]
    [SerializeField] private Transform _dropSoulPosition;
    [Header("MeshRenderer")]
    [SerializeField] private MeshRenderer _renderer;

    private void OnEnable()
    {
        EventManager.Instance.RegisterDisableGhoulArrow(this);
    }

    protected override void Start()
    {
        base.Start();
        InitializeGhoul();
    }

    private void InitializeGhoul()
    {
        _data = MonsterManager.Instance.GetMonsterData(MonsterType.Ghoul);

        m_monsterHealth = _data._health;
        m_monsterAttackPower = _data._attackPower;
        m_monsterSpeed = _data._speed;
        m_monsterAgent.speed = m_monsterSpeed;
    }



    public void OnDisableArrow(Action callBack)
    {
        
    }

    public override void TakeDamage(float damage)
    {
        
    }

}
