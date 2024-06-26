using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


public class DataManager : Singleton<DataManager>
{
    public Dictionary<int, _Monster> LoadedMonsterList { get; private set; }
    public Dictionary<int, _Boss> LoadedBossList { get; private set; }
    public Dictionary<int, _Player> LoadedPlayerList { get; private set; }
    public Dictionary<string, _PlayerSkill> LoadedPlayerSkillList { get; private set; }
    public Dictionary<string, _PlayerWeapon> LoadedPlayerWeaponList { get;private set; }

    private readonly string m_dataPath = "C:\\Users\\KGA\\Desktop\\3D_ProjectData";

    private void Awake()
    {
        ReadAllDataAwake();
    }

    private void ReadAllDataAwake()
    {
        ReadData(nameof(_Monster));
        ReadData(nameof(_Boss));
        ReadData(nameof(_Player));
        ReadData(nameof(_PlayerSkill));
        ReadData(nameof(_PlayerWeapon));
    }

    private void ReadData(string tableName)
    {
        switch(tableName)
        {
            case nameof(_Monster):
                ReadMonsterData(tableName);
                break;
            case nameof(_Boss):
                ReadBossData(tableName);
                break;
            case nameof(_Player):
                ReadPlayerData(tableName);
                break;
            case nameof(_PlayerSkill):
                ReadPlayerSkillData(tableName);
                break;
            case nameof(_PlayerWeapon):
                ReadPlayerWeaponData(tableName);
                break;
        }
    }

    private void ReadPlayerWeaponData(string tableName)
    {
        LoadedPlayerWeaponList = new Dictionary<string, _PlayerWeapon>();

        XDocument doc = XDocument.Load($"{m_dataPath}/{tableName}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempWeapon = new _PlayerWeapon();
            tempWeapon.WeaponName = data.Attribute(nameof(tempWeapon.WeaponName)).Value;
            tempWeapon.AttackPower = float.Parse(data.Attribute(nameof(tempWeapon.AttackPower)).Value);
            tempWeapon.ChargeAttackPower = float.Parse(data.Attribute(nameof(tempWeapon.ChargeAttackPower)).Value);
            tempWeapon.NormalEffectRange = float.Parse(data.Attribute(nameof(tempWeapon.NormalEffectRange)).Value);
            tempWeapon.ChargeEffectRange = float.Parse(data.Attribute(nameof(tempWeapon.ChargeEffectRange)).Value);

            string normalAttackRangeStr = data.Attribute("NormalAttackRange").Value;

            if (!string.IsNullOrEmpty(normalAttackRangeStr))
            {
                var normalAttackRange = normalAttackRangeStr.Split(',');

                var rangeValue = new float[normalAttackRange.Length];

                for (int i = 0; i < normalAttackRange.Length; i++)
                {
                    rangeValue[i] = float.Parse(normalAttackRange[i]);
                }

                Vector3 rangeVector = new Vector3(rangeValue[0], rangeValue[1], rangeValue[2]);

                tempWeapon.NormalAttackRange = rangeVector;
            }
            else
                tempWeapon.NormalAttackRange = new Vector3(0,0,0);

            string chargeAttackRangeStr = data.Attribute("ChargeAttackRange").Value;

            if(!string.IsNullOrEmpty(chargeAttackRangeStr))
            {
                var chargeAttackRange = chargeAttackRangeStr.Split(',');

                var rangeValue = new float[chargeAttackRange.Length];

                for(int i = 0; i < chargeAttackRange.Length; i++)
                {
                    rangeValue[i] = float.Parse(chargeAttackRange[i]);
                }

                Vector3 rangeVector = new Vector3(rangeValue[0], rangeValue[1], rangeValue[2]);

                tempWeapon.ChargeAttackRange = rangeVector;
            }
            else
                tempWeapon.ChargeAttackRange = new Vector3(0,0,0);

            LoadedPlayerWeaponList.Add(tempWeapon.WeaponName, tempWeapon);
        }

    }

    private void ReadPlayerSkillData(string tableName)
    {
        
    }

    private void ReadPlayerData(string tableName)
    {
        
    }

    private void ReadBossData(string tableName)
    {
        
    }

    private void ReadMonsterData(string tableName)
    {
        
    }

    public _PlayerWeapon GetWeaponData(string weaponName)
    {
        var weaponDataList = LoadedPlayerWeaponList;

        if(weaponDataList.Count == 0
            || weaponDataList.ContainsKey(weaponName) == false)
        {
            return null;
        }

        return weaponDataList[weaponName];
    }
}
