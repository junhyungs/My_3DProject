using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("ItemType")]
    public ItemType _itemType;

    [Header("UI_Position")]
    public Vector3 _uiPosition;

    protected ItemData _data;

    protected IEnumerator LoadData(string id)
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(id) == null;
        });

        var data = DataManager.Instance.GetData(id) as ItemData;

        _data = data;
    }
}
