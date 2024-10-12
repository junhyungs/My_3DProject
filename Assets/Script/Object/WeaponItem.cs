using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour, IInteractionItem
{
    [Header("ItemID")]
    [SerializeField] private PlayerWeapon _itemID;

    private ItemData _data;
    
    void Start()
    {
        string id = _itemID.ToString();

        StartCoroutine(LoadData(id));
    }

    private IEnumerator LoadData(string id)
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as ItemData;

        _data = data;
    }

    public void InteractionItem()
    {
        InventoryManager.Instance.SetWeapon(_itemID, _data);
        Debug.Log("Interaction Weapon!");
        gameObject.SetActive(false);
    }
}
