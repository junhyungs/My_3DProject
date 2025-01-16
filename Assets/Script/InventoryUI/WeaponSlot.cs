using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IWeaponCameraEvent
{
    [Header("RawImage")]
    [SerializeField] private RawImage _rawImage;

    [Header("RenderTexture")]
    [SerializeField] private RenderTexture _renderTexture;

    [Header("WeaponType")]
    [SerializeField] private PlayerWeapon _weaponType;

    private Action<PlayerWeapon, bool> _weaponAction;
    private Texture2D _texture2D;

    public ItemData Data { get; set; }
    public PlayerWeaponData WeaponData {get;set; }
    public bool OnSlot { get; set; }
    public PlayerWeapon Type => _weaponType;

    private void Awake()
    {
        Button button = gameObject.GetComponent<Button>();

        button.onClick.AddListener(OnClickChangedWeapon);
    }

    public void OnClickChangedWeapon()
    {
        if(Data == null)
        {
            return;
        }

        WeaponManager.Instance.ChangeWeapon(_weaponType);
    }

    public void InitializeWeaponSlot()
    {
        InventoryManager.Instance.RegisterWeaponEvent(this);

        _rawImage.enabled = true;

        CaptureImage();

        InvokeEvent(false);
    }
    
    public void WeaponCameraEvent(Action<PlayerWeapon, bool> callBack, bool isAdd)
    {
        if (isAdd)
        {
            _weaponAction += callBack;
        }
        else
        {
            _weaponAction -= callBack;
        }
    }

    public void LiveImage()
    {
        _rawImage.texture = _renderTexture;
    }

    public void CaptureImage()
    {
        if (_texture2D == null)
        {
            RenderTexture.active = _renderTexture;
            
            _texture2D = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);

            _texture2D.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);

            _texture2D.Apply();

            RenderTexture.active = null;
        }

        _rawImage.texture = _texture2D;
    }

    public void InvokeEvent(bool isActive)
    {
        _weaponAction?.Invoke(Type, isActive);
    }
}
