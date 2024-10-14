using System.Collections;
using UnityEngine;

public class TrinketItem : MonoBehaviour, IInteractionItem
{
    [Header("ItemID")]
    [SerializeField] private TrinketItemType _itemID;

    [Header("UI_Position")]
    [SerializeField] private Vector3 _uiPosition;

    private ItemData _data;

    private void Start()
    {
        string id = _itemID.ToString();

        StartCoroutine(LoadItemData(id));
    }

    private IEnumerator LoadItemData(string id)
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
        InventoryManager.Instance.SetItem(_itemID, _data);

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

