using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    #region MVVM
    private Action<int> PlayerSoulItemCallBack;
    private Action<int> PlayerHealthItemCallBack;

    private int m_soulCount;
    private int m_HealthCount;
    #endregion

    #region Event
    private Action<TrinketItemType, bool> _globalTrinketAction;
    private Action<PlayerWeapon, bool> _globalWeaponAction;

    private List<ITrinketCameraEvent> _trinketEventList = new List<ITrinketCameraEvent>();
    private List<IWeaponCameraEvent> _weaponEventList = new List<IWeaponCameraEvent>();

    private InventoryUI _inventoryUI;
    #endregion

    #region Data
    private Dictionary<string, ItemData> _baseDictionary;
    private Dictionary<PlayerWeapon, ItemData> _weaponDictionary;
    private Dictionary<TrinketItemType, ItemData> _trinketDictionary;

    private HashSet<ItemType> _itemTypeSet;

    private TrinketPanel _trinketPanel;
    private WeaponPanel _weaponPanel;
    #endregion

    #region Property
    public Dictionary<PlayerWeapon, ItemData> WeaponDictionary
    {
        get
        {
            if(_weaponDictionary == null)
            {
                _weaponDictionary = new Dictionary<PlayerWeapon, ItemData>();
            }

            return _weaponDictionary;
        }
    }

    public Dictionary<TrinketItemType, ItemData> TrinketDictionary
    {
        get
        {
            if(_trinketDictionary == null)
            {
                _trinketDictionary = new Dictionary<TrinketItemType, ItemData>();
            }

            return _trinketDictionary;  
        }
    }

    public Dictionary<string, ItemData> DataDictionary
    {
        get { return _baseDictionary; }
    }

    public HashSet<ItemType> ItemSet
    {
        get { return _itemTypeSet; }
    }
    #endregion

    private void Awake()
    {
        _itemTypeSet = new HashSet<ItemType>();
        _trinketPanel = transform.GetComponentInChildren<TrinketPanel>(true);
        _weaponPanel = transform.GetComponentInChildren<WeaponPanel>(true);
        _inventoryUI = GetComponent<InventoryUI>();
    }

    private void Start()
    {
        StartCoroutine(LoadItemData());
    }

    private IEnumerator LoadItemData()
    {
        _baseDictionary = new Dictionary<string, ItemData>();

        string waitKey = BaseItemType.Witch.ToString();

        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(waitKey) == null;
        });

        Array enumArray = Enum.GetValues(typeof(BaseItemType));

        for(int i = 0; i < enumArray.Length; i++)
        {
            string id = enumArray.GetValue(i).ToString();

            var data = DataManager.Instance.GetData(id) as ItemData;

            _baseDictionary.Add(id, data);
        }
    }

    #region Event
    public void RegisterTrinketEvent(ITrinketCameraEvent trinketEvent)
    {
        _trinketEventList.Add(trinketEvent); 

        if(_globalTrinketAction != null) //레지스터가 나중에 불렸지만 글로벌 액션에 메서드를 등록했기 때문에 이 글로벌 액션을 통해 바인딩함.
        {
            trinketEvent.TrinketCameraEvent(_globalTrinketAction, true);
        }
    }

    public void BindTrinketEvent(Action<TrinketItemType, bool> action)
    {
        _globalTrinketAction = action; //이 메서드가 먼저 호출된다면 글로벌 액션에 현재 바인딩 하려는 메서드를 저장함.

        foreach(var item in _trinketEventList) //레지스터 이벤트가 먼저 호출되었다면 포이치 실행.
        {
            item.TrinketCameraEvent(_globalTrinketAction, true);
        }
    }

    public void RegisterWeaponEvent(IWeaponCameraEvent weaponEvent)
    {
        _weaponEventList.Add(weaponEvent);

        if(_globalWeaponAction != null)
        {
            weaponEvent.WeaponCameraEvent(_globalWeaponAction, true);
        }
    }

    public void BindWeaponEvent(Action<PlayerWeapon, bool> action)
    {
        _globalWeaponAction = action;

        foreach(var item in _weaponEventList)
        {
            item.WeaponCameraEvent(action, true);
        }
    }

    //Start 시점에서 등록을 보장함.
    public void OnRenderWeaponObject(PlayerWeapon weapon, bool onEnable)
    {
        _globalWeaponAction?.Invoke(weapon, onEnable);
    }

    public void OnRenderTrinketObject(TrinketItemType trinketItemType, bool onEnable)
    {
        _globalTrinketAction?.Invoke(trinketItemType, onEnable);
    }

    #endregion

    public void OnInventoryTabAction(bool onAction)
    {
        _inventoryUI.ActionControl(onAction);
    }

    //무기 획득 시 호출
    public void SetWeapon(PlayerWeapon weapon, ItemType type, ItemData data, PlayerWeaponData weaponData)
    {
        if(_itemTypeSet.Contains(type))
        {
            return;
        }

        _itemTypeSet.Add(type);

        _weaponPanel.SetWeaponType(weapon, data, weaponData);
    }


    //아이템 획득 시 호출
    public void SetItem(TrinketItemType item, ItemType type, ItemData data)
    {
        if (_itemTypeSet.Contains(type))
        {
            return;
        }

        _itemTypeSet.Add(type);

        TrinketDictionary.Add(item, data);

        _trinketPanel.SetTrinketType(item, data);
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
