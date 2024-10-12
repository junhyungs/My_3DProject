using System.Collections;
using UnityEngine;

public class TrinketItem : MonoBehaviour, IInteractionItem
{
    [Header("ItemID")]
    [SerializeField] private TrinketItemType _itemID;

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
        Debug.Log("Interaction Item!");
        gameObject.SetActive(false);
    }
}
