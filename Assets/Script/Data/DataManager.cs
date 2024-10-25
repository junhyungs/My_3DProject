using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.RegularExpressions;
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
    private HashSet<string> _jsonDataSet = new HashSet<string>();

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

        if (_jsonDataSet.Contains(jsonData))
        {
            return;
        }

        switch (fileName)
        {
            case nameof(JsonName.Monster):
                ReadMonsterData(jsonData);
                break;
            case nameof(JsonName.Boss):
                ReadForestMotherData(jsonData);
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
            case nameof(JsonName.BossProjectile):
                ReadForestMotherProjectileData(jsonData);
                break;
            case nameof(JsonName.Dialogue):
                ReadDialogueData(jsonData);
                break;
            case nameof(JsonName.Item):
                ReadItemData(jsonData);
                break;
            case nameof(JsonName.Ability):
                ReadAbilityData(jsonData);
                break;
            case nameof(JsonName.Map):
                ReadMapData(jsonData);
                break;
        }
    }

    private void ReadMapData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = ParseString(item["ID"]);

                string replace = ParseString(item["MapName"]);

                string mapName = replace.Replace("\\n", "\n");

                string prefabPath = ParseString(item["PrefabPath"]);

                List<string> itemPath = MapItem(item["ItemPath"]);

                List<string> spawnMonsterList = MapMonsterList(item["SpawnMonsterList"]);

                string skyBoxPath = ParseString(item["SkyBoxPath"]);

                List<string> itemType = MapItem(item["ItemType"]);

                int spawnCount = ParseInt(item["SpawnCount"]);

                int eventCount = ParseInt(item["EventCount"]);

                MapData data = new MapData(id, mapName, prefabPath, itemPath,
                    spawnMonsterList, skyBoxPath, itemType, spawnCount, eventCount);

                _dataDictionary.Add(id, data);
            }

            _jsonDataSet.Add(jsonData);
        }
        catch(JsonException ex)
        {
            Debug.Log($"<Map> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
        }
    }

    private List<string> MapItem(JToken jtoken)
    {
        string value = jtoken.ToString();

        value = value.Trim('{', '}');

        string[] splitArray = value.Split(',');

        List<string> stringList = new List<string>();

        for(int i = 0; i < splitArray.Length; i++)
        {
            stringList.Add(splitArray[i]);
        }

        return stringList;
    }

    private List<string> MapMonsterList(JToken jtoken)
    {
        string value = jtoken.ToString();

        value = value.Trim('{', '}');

        string[] splitArray = value.Split('/');

        List<string> stringlist = new List<string>();

        for (int i = 0; i < splitArray.Length; i++)
        {
            stringlist.Add(splitArray[i]);
        }

        return stringlist;
    }
    

    private void ReadAbilityData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;

                string descriptionName = item["DescriptionName"] != null ? item["DescriptionName"].ToString() : string.Empty;

                string replace = item["Description"] != null ? item["Description"].ToString() : string.Empty;

                string description = replace.Replace("\\n", "\n");

                string priceData = item["Price"] != null ? item["Price"].ToString() : string.Empty;

                List<int> priceList = ParsePrice(priceData);

                AbilityData data = new AbilityData(id, descriptionName, description, priceList);

                _dataDictionary.Add(id, data);
            }

            _jsonDataSet.Add(jsonData);
        }
        catch (JsonException ex)
        {
            Debug.Log($"<Ability> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
        }
    }

    private List<int> ParsePrice(string priceData)
    {
        string trim = priceData.Trim('{', '}');

        string[] splitPriceData = trim.Split('/');

        List<int> priceList = new List<int>();

        for (int i = 0; i < splitPriceData.Length; i++)
        {
            int price = ParseInt(splitPriceData[i]);

            priceList.Add(price);
        }

        return priceList;
    }

    private void ReadItemData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;

                string itemName = item["ItemName"] != null ? item["ItemName"].ToString() : string.Empty;

                string Replace = item["Description"] != null ? item["Description"].ToString() : string.Empty;

                string description = Replace.Replace("\\n", "\n");

                bool equip = ParseBool(item["Equip"]);

                ItemData data = new ItemData(id, itemName, description, equip);

                _dataDictionary.Add(id, data);
            }

            _jsonDataSet.Add(jsonData);
        }
        catch(JsonException ex)
        {
            Debug.Log($"<Item> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
        }
    }

    private void ReadDialogueData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;

                string name = item["Name"] != null ? item["Name"].ToString() : string.Empty;

                List<string> stroyList = ParseDialogue(item["StoryMessage"]);

                List<string> loopList = ParseDialogue(item["LoopMessage"]);

                List<string> endMessage = ParseDialogue(item["EndMessage"]);

                DialogueData data = new DialogueData(id, name, stroyList, loopList, endMessage);

                _dataDictionary.Add(id, data);
            }

            _jsonDataSet.Add(jsonData);
        }
        catch(JsonException ex)
        {
            Debug.Log($"<Dialogue> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
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
                float agentStoppingDistance = ParseFloat(item["AgentStoppingDistance"]);
                float spawnTrackingDistance = ParseFloat(item["SpawnTrackingDistance"]);
                float spawnStopTrackingDistance = ParseFloat(item["SpawnStopTrackingDistance"]);

                BT_MonsterData monsterData = new BT_MonsterData(id, name, hp, power, speed, 
                    trackingDistance, stopTrackingDistance, agentStoppingDistance, spawnTrackingDistance, spawnStopTrackingDistance);
                _dataDictionary.Add(id, monsterData);
            }

            _jsonDataSet.Add(jsonData);
        }
        catch (JsonException ex)
        {
            Debug.Log($"<Monster> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
        }
    }

    private void ReadForestMotherData(string jsonData)
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
                float vineHealth = ParseFloat(item["VineHealth"]);
                float downHealth = ParseFloat(item["DownHealth"]);

                ForestMotherData data = new ForestMotherData(id, name, health, power, speed, vineHealth, downHealth);

                _dataDictionary.Add(id, data);
            }

            _jsonDataSet.Add(jsonData);
        }
        catch(JsonException ex)
        {
            Debug.Log($"<Boss> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
        }
    }

    private void ReadForestMotherProjectileData(string jsonData)
    {
        try
        {
            JArray jsonArray = JArray.Parse(jsonData);

            foreach(var item in jsonArray)
            {
                string id = item["ID"] != null ? item["ID"].ToString() : string.Empty;
                float forcePower = ParseFloat(item["ForcePower"]);
                float sphereRadius = ParseFloat(item["SphereRadius"]);
                float damage = ParseFloat(item["Damage"]);

                string replace = Regex.Replace(item["LayerList"].ToString(), "[{}\"]", "");
                string[] replaceArray = replace.Split('/');

                List<int>layerList = new List<int>();

                for(int i = 0; i < replaceArray.Length; i++)
                {
                    int layer = LayerMask.NameToLayer(replaceArray[i].Trim());

                    layerList.Add(layer);
                }

                ForestMotherProjectileData data = new ForestMotherProjectileData(id,forcePower,
                    sphereRadius, damage, layerList);

                _dataDictionary.Add(id, data);
            }

            _jsonDataSet.Add(jsonData);
        }
        catch(JsonException ex)
        {
            Debug.Log($"<BossProjectile> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
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

            _jsonDataSet.Add(jsonData);
        }
        catch (JsonException ex)
        {
            Debug.Log($"<Player> 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
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

            _jsonDataSet.Add(jsonData);
        }
        catch (JsonException ex)
        {
            Debug.Log($"<PlayerSkill> 스킬 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
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

            _jsonDataSet.Add(jsonData);
        }
        catch(JsonException ex)
        {
            Debug.Log($"<PlayerWeapon> 무기 데이터를 변환하지 못했습니다. 오류 내역 : {ex.Message}");
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
            }

            _jsonDataSet.Add(jsonData);
        }
        catch (JsonException ex)
        {
            Debug.Log($"<PrefabPath> 프리팹 경로 데이터를 가져오지 못했습니다. 오류 내역 : {ex.Message}");
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
            return null;
        }
    }

    private bool ParseBool(JToken json)
    {
        if(json == null)
        {
            return false;
        }

        if(json.Type == JTokenType.Boolean)
        {
            return (bool)json;
        }

        if(bool.TryParse(json.ToString(), out bool value))
        {
            return value;
        }

        Debug.Log("Bool 데이터 중 변환하지 못한 데이터가 있습니다. return false!");
        return false;
    }

    private List<string> ParseDialogue(JToken item)
    {
        string stringData = item.ToString();

        string[] splitArray = stringData.Split("{E}");

        List<string> dialogueList = new List<string>();

        foreach (var splitData in splitArray)
        {
            dialogueList.Add(splitData);
        }

        return dialogueList;
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

    private string ParseString(JToken jtoken)
    {
        string value = jtoken != null ? jtoken.ToString() : string.Empty;

        return value;
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
