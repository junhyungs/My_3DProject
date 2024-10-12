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
            Debug.Log("Enum�� ���̿� <_trinketCameraArray> �迭�� ���̰� ��ġ���� �ʽ��ϴ�.");
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
            Debug.Log("Enum�� ���̿� <_weaponCameraArray> �迭�� ���̰� ��ġ���� �ʽ��ϴ�.");
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

        Debug.Log("<TrinketCameraObject>�� ��ȯ���� ���߽��ϴ�.");
        return null;
    }

    private GameObject GetWeaponCameraObject(PlayerWeapon type)
    {
        if(_weaponCameras.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }

        Debug.Log("<WeaponCameraObject>�� ��ȯ���� ���߽��ϴ�.");
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
