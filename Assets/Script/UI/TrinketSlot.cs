using UnityEngine;
using System;
using UnityEngine.UI;

public class TrinketSlot : MonoBehaviour, ITrinketCameraEvent
{
    [Header("RawImage")]
    [SerializeField] private RawImage _rawImage;

    [Header("TrinketType")]
    [SerializeField] private TrinketItemType _type;

    private Action<TrinketItemType, bool> _trinketAction;

    private bool _onSlot;

    public ItemData Data { get; set; }
    public TrinketItemType Type
    {
        get { return _type; }
    }
 
    public bool OnSlot
    {
        get { return _onSlot; }
        set
        {
            if (!_rawImage.enabled)
            {
                InventoryManager.Instance.RegisterTrinketEvent(this);

                _rawImage.enabled = true;
            }

            _onSlot = value;
        }
    }
    

    public void TrinketCameraEvent(Action<TrinketItemType, bool> callBack, bool isAdd)
    {
        if (isAdd)
        {
            _trinketAction += callBack;
        }
        else
        {
            _trinketAction -= callBack;
        }
    }

    public void InvokeEvent(bool isActive)
    {
        _trinketAction?.Invoke(Type, isActive);
    }
}
