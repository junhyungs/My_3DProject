using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RenderCamera : MonoBehaviour
{
    [Header("TrinketCamera")]
    [SerializeField] private GameObject[] _trinketCameraArray;

    [Header("WeaponCamera")]
    [SerializeField] private GameObject[] _weaponCameraArray;

    private Dictionary<TrinketItemType, GameObject> _trinketCameras;
    private Dictionary<PlayerWeapon,  GameObject> _weaponCameras;

    private void Start()
    {
        InitializeCamera();
    }

    #region Initialize
    private void InitializeCamera()
    {
        InitializeTrinketCamera();

        InitializeWeaponCamera();
    }

    private void InitializeTrinketCamera()
    {
        Array trinketEnum = Enum.GetValues(typeof(TrinketItemType));

        if(trinketEnum.Length != _trinketCameraArray.Length)
        {
            Debug.Log("Enum의 길이와 <_trinketCameraArray> 배열의 길이가 일치하지 않습니다.");
            return;
        }

        _trinketCameras = new Dictionary<TrinketItemType, GameObject>();
        
        for(int i = 0; i < _trinketCameraArray.Length; i++)
        {
            _trinketCameraArray[i].SetActive(false);

            _trinketCameras.Add((TrinketItemType)i, _trinketCameraArray[i]);
        }

        InventoryManager.Instance.BindTrinketEvent(OnTrinketCamera);
    }

    private void InitializeWeaponCamera()
    {
        Array weaponEnum = Enum.GetValues(typeof(PlayerWeapon));

        if(weaponEnum.Length != _weaponCameraArray.Length)
        {
            Debug.Log("Enum의 길이와 <_weaponCameraArray> 배열의 길이가 일치하지 않습니다.");
            return;
        }

        _weaponCameras = new Dictionary<PlayerWeapon, GameObject>();

        for(int i = 0; i < _weaponCameraArray.Length; i++)
        {
            _weaponCameraArray[i].SetActive(false);

            _weaponCameras.Add((PlayerWeapon)i, _weaponCameraArray[i]);
        }

        InventoryManager.Instance.BindWeaponEvent(OnWeaponCamera);
    }
    #endregion

    private GameObject GetTrinketCameraObject(TrinketItemType type)
    {
        if(_trinketCameras.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }

        Debug.Log("<TrinketCameraObject>를 반환하지 못했습니다.");
        return null;
    }

    private GameObject GetWeaponCameraObject(PlayerWeapon type)
    {
        if(_weaponCameras.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }

        Debug.Log("<WeaponCameraObject>를 반환하지 못했습니다.");
        return null;
    }

    private void ResetTrinketCamera()
    {
        foreach(var camera in _trinketCameraArray)
        {
            camera.SetActive(false);
        }
    }

    private void ResetWeaponCamera()
    {
        foreach(var camera in _weaponCameraArray)
        {
            camera.SetActive(false);
        }
    }

    private void OnTrinketCamera(TrinketItemType type, bool isActive)
    {
        if (!isActive)
        {
            ResetTrinketCamera();

            return;
        }

        GameObject camera = GetTrinketCameraObject(type);

        camera.SetActive(true);
    }

    private void OnWeaponCamera(PlayerWeapon weapon, bool isActive)
    {
        if (!isActive)
        {
            ResetWeaponCamera();

            return;
        }

        GameObject camera = GetWeaponCameraObject(weapon);

        camera.SetActive(true);
    }
}
