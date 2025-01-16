using UnityEngine;
using System;
using UnityEngine.UI;

public class TrinketSlot : MonoBehaviour, ITrinketCameraEvent
{
    [Header("RawImage")]
    [SerializeField] private RawImage _rawImage;

    [Header("RenderTexture")]
    [SerializeField] private RenderTexture _renderTexture;

    [Header("TrinketType")]
    [SerializeField] private TrinketItemType _type;

    private Action<TrinketItemType, bool> _trinketAction;

    private bool _onSlot;

    private Texture2D _texture2D;

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

                CaptureImage();

                InvokeEvent(false);
            }

            _onSlot = value;
        }
    }

    public void LiveImage()
    {
        if(_renderTexture == null)
        {
            return;
        }

        _rawImage.texture = _renderTexture;
    }

    public void CaptureImage()
    {
        if(_renderTexture == null)
        {
            return;
        }

        if(_texture2D == null)
        {
            RenderTexture.active = _renderTexture;

            _texture2D = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);

            _texture2D.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);

            _texture2D.Apply();

            RenderTexture.active = null;
        }

        _rawImage.texture = _texture2D;
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
