using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item, IInteractionItem
{
    [Header("ItemID")]
    [SerializeField] private PlayerWeapon _itemID;

    [Header("DataID")]
    [SerializeField] private PlayerWeaponID _weaponID;

    private PlayerWeaponData _weaponData;
    
    void Start()
    {
        string id = _itemID.ToString();
        string weaponId = _weaponID.ToString();

        StartCoroutine(LoadData(id));
        StartCoroutine(LoadWeaponData(weaponId));
    }

    private IEnumerator LoadWeaponData(string id)
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as PlayerWeaponData;

        _weaponData = data;
    }

    public void InteractionItem()
    {
        InventoryManager.Instance.SetWeapon(_itemID, _itemType, _data, _weaponData);

        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(transform, _uiPosition, ObjectName.GetUI);

            InventoryManager.Instance.OnRenderWeaponObject(_itemID, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);

            InventoryManager.Instance.OnRenderWeaponObject(_itemID, false);
        }
    }
}
