using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class Item_SO : ScriptableObject
{
    private int m_ItemID;
    
    private string m_Item_Description;

    public int ItemId
    {
        get { return m_ItemID; }
        set { m_ItemID = value; }
    }

    

    public string Item_Description
    {
        get { return m_Item_Description; }
        set { m_Item_Description = value;}
    }

    

}
