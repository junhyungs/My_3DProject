using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class Item_SO : ScriptableObject
{
    private int m_ItemID;
    private ItemType m_ItemType;
    private string m_Item_Description;

    public int ItemId
    {
        get { return m_ItemID; }
        set { m_ItemID = value; }
    }

    public ItemType ItemType
    {
        get { return m_ItemType; }
        set { m_ItemType = value; } 
    }

    public string Item_Description
    {
        get { return m_Item_Description; }
        set { m_Item_Description = value;}
    }

    public void SetItemData(int itemId, ItemType itemType, string itemDescription)
    {
        m_ItemID = itemId;
        m_ItemType = itemType;
        m_Item_Description = itemDescription;
    }

}
