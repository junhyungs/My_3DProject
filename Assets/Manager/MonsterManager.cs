using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Bat = 1,
    Slime,
    Mage,
    Pot,
    Deku,
    Ghoul
}

public class MonsterManager : Singleton<MonsterManager>
{
    private Dictionary<MonsterType, MonsterData> _monsterDataDictionary = new Dictionary<MonsterType, MonsterData>();

    private void Awake()
    {
        InitializeMonsterData(MonsterType.Bat, (int)MonsterType.Bat);
        InitializeMonsterData(MonsterType.Slime, (int)MonsterType.Slime);
        InitializeMonsterData(MonsterType.Mage, (int)MonsterType.Mage);
        InitializeMonsterData(MonsterType.Pot, (int)MonsterType.Pot);
        InitializeMonsterData(MonsterType.Deku, (int)MonsterType.Deku);
        InitializeMonsterData(MonsterType.Ghoul, (int)MonsterType.Ghoul);
    }

    private void InitializeMonsterData(MonsterType monsterType, int id)
    {
        var monsterData = DataManager.Instance.GetMonsterData(id);

        MonsterData data = new MonsterData(
            monsterData.MonsterId,
            monsterData.MonsterName,
            monsterData.MonsterHp,
            monsterData.MonsterAttackPower,
            monsterData.MonsterSpeed
            );

        _monsterDataDictionary.Add(monsterType, data);
    }

    public MonsterData GetMonsterData(MonsterType type)
    {
        if(_monsterDataDictionary.TryGetValue(type, out MonsterData data))
        {
            return data;
        }

        return new MonsterData(0, "Default", 1, 1, 1f);
    }
}


public struct MonsterData
{
    public int _id { get; }
    public string _name { get; }
    public int _health {  get; }
    public int _attackPower { get; }
    public float _speed {  get; }

    public MonsterData(int id, string name, int health, int attackPower, float speed)
    {
        _id = id;
        _name = name;
        _health = health;
        _attackPower = attackPower;
        _speed = speed;
        _attackPower = attackPower;
    }
}