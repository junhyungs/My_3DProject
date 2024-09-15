
using System.Collections.Generic;
using System.Numerics;
public class Data { }

public class BT_MonsterData : Data
{
    public string ID { get; set; }
    public string Name { get; set; }    
    public int Health { get; set; }
    public float Power { get; set; } 
    public float Speed { get; set; }    

    public BT_MonsterData(string id, string name, int health, float power, float speed)
    {
        ID = id;
        Name = name;
        Health = health;
        Power = power;
        Speed = speed;
    }
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

public class BT_BossData : Data
{
    public string ID { get; set; }
    public string Name { get; set; }
    public float Health {  get; set; }
    public float Power { get; set; }
    public float Speed { get; set; }

    public BT_BossData(string iD, string name, float health, float power, float speed)
    {
        ID=iD;
        Name=name;
        Health=health;
        Power=power;
        Speed=speed;
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

public class _Monster
{
    public int MonsterId { get; set; }
    public string MonsterName { get; set; }
    public int MonsterHp { get; set; }
    public int MonsterAttackPower { get; set; }
    public float MonsterSpeed { get; set; }
}

public class _Boss
{
    public int BossId { get; set; }
    public string BossName { get; set;}
    public float BossHp { get; set;}
    public float BossAttackPower { get;set;}
    public float BossSpeed { get; set;}
}

public class _PlayerSkill
{
    public string SkillName { get; set; }
    public float SkillAttackPower { get; set; }
    public float ProjectileSpeed { get; set; }
    public float SkillRange { get; set; }
    public int Cost { get; set; }
}

public class _PlayerWeapon
{
    public string WeaponName { get; set; }
    public float AttackPower { get; set; }
    public float ChargeAttackPower {  get; set; }   
    public float NormalEffectRange { get; set; }
    public float ChargeEffectRange {  get; set; }
    public UnityEngine.Vector3 NormalAttackRange { get; set; }
    public UnityEngine.Vector3 ChargeAttackRange { get; set; }
}
