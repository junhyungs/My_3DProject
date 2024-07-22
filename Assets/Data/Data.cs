
using System.Numerics;

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
