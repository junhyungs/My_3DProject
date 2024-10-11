using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private int m_soulCount;
    private int m_HealthCount;

    private Action<int> PlayerSoulItemCallBack;
    private Action<int> PlayerHealthItemCallBack;

    private Dictionary<string, ItemData> _dataDictionary;
    private HashSet<PlayerWeapon> _weaponSet;
    private HashSet<ItemType> _itemSet;

    public Dictionary<string, ItemData> DataDictionary
    {
        get { return _dataDictionary; }
    }

    public HashSet<PlayerWeapon> WeaponSet
    {
        get { return _weaponSet; }
    }

    public HashSet<ItemType> ItemSet
    {
        get { return _itemSet; }
    }

    private void Awake()
    {
        _weaponSet = new HashSet<PlayerWeapon>();

        _itemSet = new HashSet<ItemType>();

        SetWeapon(PlayerWeapon.Sword);
    }

    private void Start()
    {
        StartCoroutine(LoadItemData());
    }

    private IEnumerator LoadItemData()
    {
        _dataDictionary = new Dictionary<string, ItemData>();

        string waitKey = DescriptionType.Witch.ToString();

        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(waitKey) == null;
        });

        Array enumArray = Enum.GetValues(typeof(DescriptionType));

        for(int i = 0; i < enumArray.Length; i++)
        {
            string id = enumArray.GetValue(i).ToString();

            var data = DataManager.Instance.GetData(id) as ItemData;

            _dataDictionary.Add(id, data);
        }
    }

    //¹«±â È¹µæ ½Ã È£Ãâ
    public void SetWeapon(PlayerWeapon weapon)
    {
        if(!_weaponSet.Contains(weapon))
        {
            _weaponSet.Add(weapon);
        }
    }

    //¾ÆÀÌÅÛ È¹µæ ½Ã È£Ãâ
    public void SetItem(ItemType item)
    {
        if (!_itemSet.Contains(item))
        {
            _itemSet.Add(item);
        }
    }

    public void SetSoulCount(int soulCount)
    {
        m_soulCount += soulCount;
        RequestChangeSoulValue(m_soulCount);
    }

    public void SetHealthCount(int healthCount)
    {
        m_HealthCount += healthCount;
        RequestChangeHealthValue(m_HealthCount);
    }

    public bool UseHealthItem()
    {
        if (m_HealthCount != 0)
        {
            m_HealthCount--;
            
            RequestChangeHealthValue(m_HealthCount);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UseSoul(int value)
    {
        if (m_soulCount == 0)
            return false;

        int currentSoul = m_soulCount;

        int afterUseSoul = currentSoul - value;

        if (afterUseSoul >= 0)
        {
            m_soulCount = afterUseSoul;

            return true;
        }
        else
            return false;
    }

    #region MVVM
    //Soul
    public void RequestChangeSoulValue(int value)
    {
        PlayerSoulItemCallBack?.Invoke(value);
    }

    public void RegisterChangeSoulValueCallBack(Action<int> valueCallBack)
    {
        PlayerSoulItemCallBack += valueCallBack;
    }

    public void UnRegisterChangeSoulValueCallBack(Action<int> valueCallBack)
    {
        PlayerSoulItemCallBack -= valueCallBack;
    }

    //Health
    public void RequestChangeHealthValue(int value)
    {
        PlayerHealthItemCallBack?.Invoke(value);
    }

    public void RegisterChangeHealthValueCallBack(Action<int> valueCallBack)
    {
        PlayerHealthItemCallBack += valueCallBack;
    }

    public void UnRegisterChangeHealthValueCallBack(Action<int> valueCallBack)
    {
        PlayerHealthItemCallBack -= valueCallBack;
    }
    #endregion
}
