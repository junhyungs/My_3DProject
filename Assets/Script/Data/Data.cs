
using System.Collections.Generic;
using System.Numerics;
public class Data { }
public class PathData { }

public class BT_MonsterData : Data
{
    public string ID { get; set; }
    public string Name { get; set; }    
    public int Health { get; set; }
    public float Power { get; set; } 
    public float Speed { get; set; }
    public float TrackingDistance { get; set; }
    public float StopTrackingDistance { get; set; }


    public BT_MonsterData(string id, string name, int health, float power, float speed, float trackingDistance, float stopTrackingDistance)
    {
        ID = id;
        Name = name;
        Health = health;
        Power = power;
        Speed = speed;
        TrackingDistance = trackingDistance;
        StopTrackingDistance = stopTrackingDistance;
    }
}

public class BT_MonsterDropObjectData : Data
{
    public string ID { get; set; }
    public Vector3 Scale { get; set; }
}

public class PlayerData : Data
{
    public string ID {  get; set; }
    public string Name { get; set; } 
    public float Power { get; set; }
    public float Speed {  get; set; }
    public float RollSpeed { get; set; }
    public float LadderSpeed { get; set; }
    public float SpeedChangeValue { get; set; }
    public float SpeedOffSet {  get; set; } 
    public float Gravity { get; set; }
    public int Health { get; set; }

    public PlayerData(string iD, string name, float power, float speed, float rollSpeed, float ladderSpeed, float speedChangeValue, float speedOffSet, float gravity, int health)
    {
        ID = iD;
        Name = name;
        Power = power;
        Speed = speed;
        RollSpeed = rollSpeed;
        LadderSpeed = ladderSpeed;
        SpeedChangeValue = speedChangeValue;
        SpeedOffSet = speedOffSet;
        Gravity = gravity;
        Health = health;
    }
}

public class ForestMotherData : Data
{
    public string ID { get; set; }
    public string Name { get; set; }
    public float Health {  get; set; }
    public float Power { get; set; }
    public float Speed { get; set; }
    public float VineHealth { get; set; }
    public float DownHealth { get; set; }

    public ForestMotherData(string iD, string name, float health, 
        float power, float speed, float vineHealth, float downHealth)
    {
        ID = iD;
        Name = name;
        Health = health;
        Power = power;
        Speed = speed;
        VineHealth = vineHealth;  
        DownHealth = downHealth;
    }
}

public class PlayerSkillData : Data
{
    public string ID { get; set; }
    public string Name { get; set; }
    public float Power { get; set; }
    public float ProjectileSpeed { get; set; }
    public float SkillRange { get; set; }
    public int Cost {  get; set; }

    public PlayerSkillData(string iD, string name, float power, float projectileSpeed, float skillRange, int cost)
    {
        ID=iD;
        Name=name;
        Power=power;
        ProjectileSpeed=projectileSpeed;
        SkillRange=skillRange;
        Cost=cost;
    }
}

public class PlayerWeaponData : Data
{
    public string ID { get; set; }
    public string Name { get; set; }
    public float Power { get; set; }
    public float ChargePower { get; set; }
    public float NormalEffectRange { get; set; }    
    public float ChargeEffectRange { get; set; }
    public UnityEngine.Vector3 NormalAttackRange { get; set; }
    public UnityEngine.Vector3 ChargeAttackRange { get; set; }
    public float EffectCount { get; set; }

    public PlayerWeaponData(string iD, string name, float power, float chargePower, float normalEffectRange, float chargeEffectRange, UnityEngine.Vector3 normalAttackRange, 
        UnityEngine.Vector3 chargeAttackRange, float effectCount)
    {
        ID=iD;
        Name=name;
        Power=power;
        ChargePower=chargePower;
        NormalEffectRange=normalEffectRange;
        ChargeEffectRange=chargeEffectRange;
        NormalAttackRange=normalAttackRange;
        ChargeAttackRange=chargeAttackRange;
        EffectCount=effectCount;
    }
}

public class PrefabPath : PathData
{
    public string Id { get; set; }
    public string Path { get; set; }

    public PrefabPath(string id, string path)
    {
        Id = id;
        Path = path;
    }
}

public class ItemData : Data
{
    public string ID { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public bool Equip { get; set; }

    public ItemData(string iD, string itemName, string description, bool equip)
    {
        ID = iD;
        ItemName = itemName;
        Description = description;
        Equip = equip;
    }
}