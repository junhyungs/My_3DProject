
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
    public float AgentStoppingDistance { get; set; }
    public float SpawnTrackingDistance { get; set; }
    public float SpawnStopTrackingDistance { get; set; }

    public BT_MonsterData(string id, string name, int health, float power, float speed, 
        float trackingDistance, float stopTrackingDistance, float agentStoppingDistance,
        float spawnTrackingDistance, float spawnStopTrackingDistance)
    {
        ID = id;
        Name = name;
        Health = health;
        Power = power;
        Speed = speed;
        TrackingDistance = trackingDistance;
        StopTrackingDistance = stopTrackingDistance;
        AgentStoppingDistance = agentStoppingDistance;
        SpawnTrackingDistance = spawnTrackingDistance;
        SpawnStopTrackingDistance = spawnStopTrackingDistance;
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

    public PlayerData(string iD, string name, float power, float speed, float rollSpeed, 
        float ladderSpeed, float speedChangeValue, float speedOffSet, float gravity, int health)
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

public class ForestMotherProjectileData : Data
{
    public string ID { get; set; }
    public float ForcePower { get; set; }
    public float SphereRadius { get; set; }
    public float Damage { get; set; }
    public List<int> LayerList { get; set; }
    public ForestMotherProjectileData(string iD, float forcePower, float sphereRadius,
        float damage, List<int> layerList)
    {
        ID = iD;
        ForcePower = forcePower;
        SphereRadius = sphereRadius;
        Damage = damage;
        LayerList = layerList;
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

public class DialogueData : Data
{
    public string ID { get; set; }
    public string Name { get; set; }
    public List<string> StoryMessage { get; set; }
    public List<string> LoopMessage { get; set; }
    public List<string> EndMessage { get;set; }

    public DialogueData(string iD, string name, List<string> storyMessage,
        List<string> loopMessage, List<string> endMessage)
    {
        ID = iD;
        Name = name;
        StoryMessage = storyMessage;
        LoopMessage = loopMessage;
        EndMessage = endMessage;
    }
}

public class AbilityData : Data
{
    public string ID { get; set; }
    public string DescriptionName { get; set; }
    public string Description { get; set; }
    public List<int> PriceList { get; set; }

    public AbilityData(string iD, string descriptionName, string description, List<int> priceList)
    {
        ID = iD;
        DescriptionName = descriptionName;
        Description = description;
        PriceList = priceList;
    }
}

public class MapData : Data
{
    public string ID { get; set; }
    public List<string> MapName { get; set; }
    public string PrefabPath { get; set; }
    public List<string> ItemPath { get; set; }
    public List<string> SpawnMonsterList { get; set; }
    public string SkyBoxPath { get; set; }
    public List<string> ItemType { get; set; }
    public int SpawnCount { get; set; }
    public int EventCount { get; set; }

    public MapData(string iD, List<string> mapName, string prefabPath, List<string> itemList,
        List<string> spawnMonsterList, string skyBoxPath, List<string> itemType, int spawnCount, int eventCount)
    {
        ID = iD;
        MapName = mapName;
        PrefabPath = prefabPath;
        ItemPath = itemList;
        SpawnMonsterList = spawnMonsterList;
        SkyBoxPath = skyBoxPath;
        ItemType = itemType;
        SpawnCount = spawnCount;
        EventCount = eventCount;
    }
}