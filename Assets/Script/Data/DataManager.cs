using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class DataManager
{
    private static DataManager instance;
    //public Dictionary<int, _Monster> LoadedMonsterList { get; private set; }
    //public Dictionary<int, _Boss> LoadedBossList { get; private set; }
    //public Dictionary<string, _PlayerSkill> LoadedPlayerSkillList { get; private set; }
    //public Dictionary<string, _PlayerWeapon> LoadedPlayerWeaponList { get; private set; }

    private Dictionary<string, Data> _dataDictionary = new Dictionary<string, Data>(); 
    private Dictionary<string, PathData> _pathDictionary = new Dictionary<string, PathData>();

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }

            return instance;
        }
    }

    public void SetData(string fileName, string jsonData)
    {

        switch (fileName)
        {
            case nameof(JsonName.Monster):
                ReadMonsterData(jsonData);
                break;
            case nameof(JsonName.Boss):
                ReadBossData(jsonData);
                break;
            case nameof(JsonName.Player):
                ReadPlayerData(jsonData);
                break;
            case nameof(JsonName.PlayerSkill):
                ReadPlayerSkillData(jsonData);
                break;
            case nameof(JsonName.PlayerWeapon):
                ReadPlayerWeaponData(jsonData);
                break;
            case nameof(JsonName.PrefabPath):
                ReadPrefabPathData(jsonData);
                break;
        }
    }


    private void ReadMonsterData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach (var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;
                string name = item["Name"] != null ? item["Name"].ToString() : string.Empty;
                int hp = ParseInt(item["HP"]);
                float power = ParseFloat(item["Power"]);
                float speed = ParseFloat(item["Speed"]);
                float trackingDistance = ParseFloat(item["TrackingDistance"]);
                float stopTrackingDistance = ParseFloat(item["StopTrackingDistance"]);

                BT_MonsterData monsterData = new BT_MonsterData(id, name, hp, power, speed, 
                    trackingDistance, stopTrackingDistance);
                _dataDictionary.Add(id, monsterData);
            }
        }
        catch (JsonException ex)
        {
            Debug.Log("<Monster> 데이터를 변환하지 못했습니다.");
        }
    }

    private void ReadBossData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;
                string name = item["Name"] != null ? item["Name"].ToString() : string.Empty;
                float health = ParseFloat(item["Health"]);
                float power = ParseFloat(item["Power"]);
                float speed = ParseFloat(item["Speed"]);

                BT_BossData data = new BT_BossData(id, name, health, power, speed);

                _dataDictionary.Add(id, data);
            }
        }
        catch(JsonException ex)
        {
            Debug.Log("<Boss> 데이터를 변환하지 못했습니다.");
        }
    }

    private void ReadPlayerData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach (var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;
                string name = item["Name"] != null ? item["Name"].ToString() : string.Empty;
                float power = ParseFloat(item["Power"]);
                float speed = ParseFloat(item["Speed"]);
                float rollSpeed = ParseFloat(item["RollSpeed"]);
                float ladderSpeed = ParseFloat(item["LadderSpeed"]);
                float speedChangeValue = ParseFloat(item["SpeedChangeValue"]);
                float speedOffset = ParseFloat(item["SpeedOffSet"]);
                float gravity = ParseFloat(item["Gravity"]);
                int health = ParseInt(item["Health"]);

                PlayerData playerData = new PlayerData(id, name, power, speed, rollSpeed, ladderSpeed, speedChangeValue, speedOffset
                    , gravity, health);
                _dataDictionary.Add(id, playerData);
            }
        }
        catch (JsonException ex)
        {
            Debug.Log("<Player> 데이터를 변환하지 못했습니다.");
        }
    }

    private void ReadPlayerSkillData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach (var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;
                string name = item["Name"] != null ? item["Name"].ToString() : string.Empty;
                float power = ParseFloat(item["Power"]);
                float projectileSpeed = ParseFloat(item["ProjectileSpeed"]);
                float skillRange = ParseFloat(item["SkillRange"]);
                int cost = ParseInt(item["Cost"]);

                PlayerSkillData skillData = new PlayerSkillData(id,name, power, projectileSpeed, skillRange, cost); 
                _dataDictionary.Add(id, skillData); 
            }
        }
        catch (JsonException ex)
        {
            Debug.Log("<PlayerSkill> 스킬 데이터를 변환하지 못했습니다.");
        }
    }

    private void ReadPlayerWeaponData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() :string.Empty;
                string name = item["Name"] != null ? item["Name"].ToString() : string.Empty;
                float power = ParseFloat(item["Power"]);
                float chargePower = ParseFloat(item["ChargePower"]);
                float normalEffectrange = ParseFloat(item["NormalEffectRange"]);
                float chargeEffectrange = ParseFloat(item["ChargeEffectRange"]);
                Vector3 normalAttackRange = ParseVector3(item["NormalAttackRange"]);
                Vector3 chargeAttackRange = ParseVector3(item["ChargeAttackRange"]);
                float effectCount = ParseFloat(item["EffectCount"]);

                PlayerWeaponData weaponData = new PlayerWeaponData(id, name, power, chargePower, normalEffectrange, 
                    chargeEffectrange, normalAttackRange, chargeAttackRange, effectCount);

                _dataDictionary.Add(id, weaponData);
            }
        }
        catch(JsonException ex)
        {
            Debug.Log("<PlayerWeapon> 무기 데이터를 변환하지 못했습니다.");
        }
    }

    private void ReadPrefabPathData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;
                string path = item["Path"] != null ? item["Path"].ToString() : string.Empty;

                PrefabPath pathData = new PrefabPath(id, path);
                _pathDictionary.Add(id, pathData);  
                Debug.Log(pathData.Path);
            }
        }
        catch (JsonException ex)
        {
            Debug.Log("<PrefabPath> 프리팹 경로 데이터를 가져오지 못했습니다.");
        }
    }

    public Data GetData(string Id)
    {
        if (_dataDictionary.TryGetValue(Id, out Data data))
        {
            return data;
        }
        else
        {
            Debug.Log("딕셔너리에서 데이터를 가져오지 못했습니다.");
            return null;
        }
    }

    public PathData GetPath(string Id)
    {
        if(_pathDictionary.TryGetValue(Id, out PathData data))
        {
            return data;
        }
        else
        {
            Debug.Log("경로 데이터를 가져오지 못했습니다.");
            return null;
        }
    }


    private float ParseFloat(JToken json)
    {
        if (json == null || !float.TryParse(json.ToString(), out float value))
        {
            value = 0f;
        }

        return value;
    }

    private int ParseInt(JToken json)
    {
        if (json == null || !int.TryParse(json.ToString(), out int value))
        {
            value = 0;
        }
        
        return value;
    }

    private Vector3 ParseVector3(JToken json)
    {
       string data = json.ToString();

        if (!string.IsNullOrWhiteSpace(data))
        {
            var range = data.Split(',');

            var floatArray = new float[range.Length];

            for (int i = 0; i < range.Length; i++)
            {
                floatArray[i] = float.Parse(range[i]);
            }

            Vector3 rangeVector = new Vector3(floatArray[0], floatArray[1], floatArray[2]);

            return rangeVector;
        }

        return new Vector3(0f, 0f, 0f);
    }

    #region XML
    //public void ReadAllDataAwake()
    //{
    //    ReadData(nameof(_Monster));
    //    ReadData(nameof(_Boss));
    //    ReadData(nameof(_PlayerSkill));
    //    ReadData(nameof(_PlayerWeapon));
    //}

    //private void ReadData(string tableName)
    //{
    //    switch (tableName)
    //    {
    //        case nameof(_Monster):
    //            ReadMonsterData(tableName);
    //            break;
    //        case nameof(_Boss):
    //            ReadBossData(tableName);
    //            break;
    //        case nameof(_PlayerSkill):
    //            ReadPlayerSkillData(tableName);
    //            break;
    //        case nameof(_PlayerWeapon):
    //            ReadPlayerWeaponData(tableName);
    //            break;
    //    }
    //}

    //private void ReadPlayerWeaponData(string tableName)
    //{
    //    LoadedPlayerWeaponList = new Dictionary<string, _PlayerWeapon>();

    //    XDocument doc = XDocument.Load($"{m_dataPath}/{tableName}.xml");
    //    var dataElements = doc.Descendants("data");

    //    foreach (var data in dataElements)
    //    {
    //        var tempWeapon = new _PlayerWeapon();
    //        tempWeapon.WeaponName = data.Attribute(nameof(tempWeapon.WeaponName)).Value;
    //        tempWeapon.AttackPower = float.Parse(data.Attribute(nameof(tempWeapon.AttackPower)).Value);
    //        tempWeapon.ChargeAttackPower = float.Parse(data.Attribute(nameof(tempWeapon.ChargeAttackPower)).Value);
    //        tempWeapon.NormalEffectRange = float.Parse(data.Attribute(nameof(tempWeapon.NormalEffectRange)).Value);
    //        tempWeapon.ChargeEffectRange = float.Parse(data.Attribute(nameof(tempWeapon.ChargeEffectRange)).Value);

    //        string normalAttackRangeStr = data.Attribute("NormalAttackRange").Value;

    //        if (!string.IsNullOrEmpty(normalAttackRangeStr))
    //        {
    //            var normalAttackRange = normalAttackRangeStr.Split(',');

    //            var rangeValue = new float[normalAttackRange.Length];

    //            for (int i = 0; i < normalAttackRange.Length; i++)
    //            {
    //                rangeValue[i] = float.Parse(normalAttackRange[i]);
    //            }

    //            Vector3 rangeVector = new Vector3(rangeValue[0], rangeValue[1], rangeValue[2]);

    //            tempWeapon.NormalAttackRange = rangeVector;
    //        }
    //        else
    //            tempWeapon.NormalAttackRange = new Vector3(0, 0, 0);

    //        string chargeAttackRangeStr = data.Attribute("ChargeAttackRange").Value;

    //        if (!string.IsNullOrEmpty(chargeAttackRangeStr))
    //        {
    //            var chargeAttackRange = chargeAttackRangeStr.Split(',');

    //            var rangeValue = new float[chargeAttackRange.Length];

    //            for (int i = 0; i < chargeAttackRange.Length; i++)
    //            {
    //                rangeValue[i] = float.Parse(chargeAttackRange[i]);
    //            }

    //            Vector3 rangeVector = new Vector3(rangeValue[0], rangeValue[1], rangeValue[2]);

    //            tempWeapon.ChargeAttackRange = rangeVector;
    //        }
    //        else
    //            tempWeapon.ChargeAttackRange = new Vector3(0, 0, 0);

    //        LoadedPlayerWeaponList.Add(tempWeapon.WeaponName, tempWeapon);
    //    }

    //}


    //private void ReadPlayerSkillData(string tableName)
    //{
    //    LoadedPlayerSkillList = new Dictionary<string, _PlayerSkill>();

    //    XDocument doc = XDocument.Load($"{m_dataPath}/{tableName}.xml");
    //    var dataElements = doc.Descendants("data");

    //    foreach (var data in dataElements)
    //    {
    //        var tempSkill = new _PlayerSkill();

    //        tempSkill.SkillName = data.Attribute(nameof(tempSkill.SkillName)).Value;
    //        tempSkill.SkillAttackPower = float.Parse(data.Attribute(nameof(tempSkill.SkillAttackPower)).Value);
    //        tempSkill.ProjectileSpeed = float.Parse(data.Attribute(nameof(tempSkill.ProjectileSpeed)).Value);
    //        tempSkill.SkillRange = float.Parse(data.Attribute(nameof(tempSkill.SkillRange)).Value);
    //        tempSkill.Cost = int.Parse(data.Attribute(nameof(tempSkill.Cost)).Value);

    //        LoadedPlayerSkillList.Add(tempSkill.SkillName, tempSkill);
    //    }
    //}

    //private void ReadBossData(string tableName)
    //{
    //    LoadedBossList = new Dictionary<int, _Boss>();

    //    XDocument doc = XDocument.Load($"{m_dataPath}/{tableName}.xml");
    //    var dataElements = doc.Descendants("data");

    //    foreach (var data in dataElements)
    //    {
    //        var tempBoss = new _Boss();

    //        tempBoss.BossId = int.Parse(data.Attribute(nameof(tempBoss.BossId)).Value);
    //        tempBoss.BossName = data.Attribute(nameof(tempBoss.BossName)).Value;
    //        tempBoss.BossHp = float.Parse(data.Attribute(nameof(tempBoss.BossHp)).Value);
    //        tempBoss.BossAttackPower = float.Parse(data.Attribute(nameof(tempBoss.BossAttackPower)).Value);
    //        tempBoss.BossSpeed = float.Parse(data.Attribute(nameof(tempBoss.BossSpeed)).Value);

    //        LoadedBossList.Add(tempBoss.BossId, tempBoss);

    //    }
    //}

    //private void ReadMonsterData(string tableName)
    //{
    //    LoadedMonsterList = new Dictionary<int, _Monster>();

    //    XDocument doc = XDocument.Load($"{m_dataPath}/{tableName}.xml");
    //    var dataElements = doc.Descendants("data");

    //    foreach (var data in dataElements)
    //    {
    //        var tempMonster = new _Monster();

    //        tempMonster.MonsterId = int.Parse(data.Attribute(nameof(tempMonster.MonsterId)).Value);
    //        tempMonster.MonsterName = data.Attribute(nameof(tempMonster.MonsterName)).Value;
    //        tempMonster.MonsterHp = int.Parse(data.Attribute(nameof(tempMonster.MonsterHp)).Value);
    //        tempMonster.MonsterAttackPower = int.Parse(data.Attribute(nameof(tempMonster.MonsterAttackPower)).Value);
    //        tempMonster.MonsterSpeed = float.Parse(data.Attribute(nameof(tempMonster.MonsterSpeed)).Value);

    //        LoadedMonsterList.Add(tempMonster.MonsterId, tempMonster);
    //    }
    //}

    //public _PlayerWeapon GetWeaponData(string weaponName)
    //{
    //    var weaponDataList = LoadedPlayerWeaponList;

    //    if (weaponDataList.Count == 0
    //        || weaponDataList.ContainsKey(weaponName) == false)
    //    {
    //        return null;
    //    }

    //    return weaponDataList[weaponName];
    //}

    //public _Monster GetMonsterData(int monsterId)
    //{
    //    var monsterDataList = LoadedMonsterList;

    //    if (monsterDataList.Count == 0
    //        || monsterDataList.ContainsKey(monsterId) == false)
    //    {
    //        return null;
    //    }

    //    return monsterDataList[monsterId];
    //}

    //public _Boss GetBossData(int bossId)
    //{
    //    var bossDataList = LoadedBossList;

    //    if (bossDataList.Count == 0
    //        || bossDataList.ContainsKey(bossId) == false)
    //    {
    //        return null;
    //    }

    //    return bossDataList[bossId];
    //}

    //public _PlayerSkill GetSkillData(string skillName)
    //{
    //    var skillDataList = LoadedPlayerSkillList;

    //    if (skillDataList.Count == 0
    //        || skillDataList.ContainsKey(skillName) == false)
    //    {
    //        return null;
    //    }

    //    return skillDataList[skillName];
    //}
    #endregion
}
