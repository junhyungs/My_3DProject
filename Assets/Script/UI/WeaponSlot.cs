using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IWeaponCameraEvent
{
    [Header("RawImage")]
    [SerializeField] private RawImage _rawImage;

    [Header("WeaponType")]
    [SerializeField] private PlayerWeapon _weaponType;

    private Action<PlayerWeapon, bool> _weaponAction;

    private bool _onSlot = false;

    public ItemData Data { get; set; }
    public PlayerWeaponData WeaponData {get;set; }

    public PlayerWeapon Type
    {
        get { return _weaponType; }
    }

    public bool OnSlot
    {
        get { return _onSlot; }
        set
        {
            if (!_rawImage.enabled)
            {
                InventoryManager.Instance.RegisterWeaponEvent(this);

                _rawImage.enabled = true;
            }

            _onSlot = value;
        }
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

    public void InvokeEvent(bool isActive)
    {
        _weaponAction?.Invoke(Type, isActive);
    }
}
