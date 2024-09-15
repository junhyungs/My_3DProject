using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    OldCrow = 1,
    ForestMother
}

public class BossManager : Singleton<BossManager>
{
    private Dictionary<BossType, BossData> _bossDataDictionary = new Dictionary<BossType, BossData>();

    private void Awake()
    {
       // InitializeBossData(BossType.OldCrow, (int)BossType.OldCrow);
    }

    //private void InitializeBossData(BossType bossType, int id)
    //{
    //    var bossData = DataManager.Instance.GetBossData(id);

    //    BossData data = new BossData(
    //        bossData.BossId,
    //        bossData.BossName,
    //        bossData.BossHp,
    //        bossData.BossAttackPower,
    //        bossData.BossSpeed
    //        );

    //    _bossDataDictionary.Add(bossType, data);
    //}

    public BossData GetBossData(BossType type)
    {
        return _bossDataDictionary[type];   
    }
}

public struct BossData
{
    public int _id { get; }
    public string _name { get; }
    public float _hp { get; }
    public float _power { get; }
    public float _speed { get; }

    public BossData(int id, string name, float hp, float power, float speed)
    {
        _id = id;
        _name = name;
        _hp = hp;
        _power = power;
        _speed = speed;
    }
}