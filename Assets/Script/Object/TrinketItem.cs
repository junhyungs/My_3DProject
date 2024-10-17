using System.Collections;
using UnityEngine;

public class TrinketItem : Item, IInteractionItem
{
    [Header("ItemID")]
    [SerializeField] private TrinketItemType _itemID;

    private void Start()
    {
        string id = _itemID.ToString();

        StartCoroutine(LoadData(id));
    }

    public void InteractionItem()
    {
        InventoryManager.Instance.SetItem(_itemID, _itemType, _data);

        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(transform, _uiPosition, ObjectName.GetUI);

            InventoryManager.Instance.OnRenderTrinketObject(_itemID, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);

            InventoryManager.Instance.OnRenderTrinketObject(_itemID, false);
        }
    }
}

