using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RenderCamera : MonoBehaviour
{
    [Header("TrinketRenderObject")]
    [SerializeField] private GameObject[] _trinketRenderObjectArray;

    [Header("WeaponRenderObject")]
    [SerializeField] private GameObject[] _weaponRenderObjectArray;

    [Header("TrinketCamera")]
    [SerializeField] private Camera _trinketCamera;

    [Header("WeaponCamera")]
    [SerializeField] private Camera _weaponCamera;  

    private Dictionary<TrinketItemType, GameObject> _trinketDictionary;
    private Dictionary<PlayerWeapon, GameObject> _weaponDictionary;

    private void Start()
    {
        InitializeCamera();
    }

    #region Initialize
    private void InitializeCamera()
    {
        InitializeTrinketObject();

        InitializeWeaponObject();
    }

    private void InitializeTrinketObject()
    {
        Array trinketEnum = Enum.GetValues(typeof(TrinketItemType));

        if(trinketEnum.Length != _trinketRenderObjectArray.Length)
        {
            Debug.Log("Enum의 길이와 <_trinketCameraArray> 배열의 길이가 일치하지 않습니다.");
            return;
        }

        _trinketDictionary = new Dictionary<TrinketItemType, GameObject>();
        
        for(int i = 0; i < _trinketRenderObjectArray.Length; i++)
        {
            _trinketRenderObjectArray[i].SetActive(false);

            _trinketDictionary.Add((TrinketItemType)i, _trinketRenderObjectArray[i]);
        }

        InventoryManager.Instance.BindTrinketEvent(OnTrinketCamera);
    }

    private void InitializeWeaponObject()
    {
        Array weaponEnum = Enum.GetValues(typeof(PlayerWeapon));

        if(weaponEnum.Length != _weaponRenderObjectArray.Length)
        {
            Debug.Log("Enum의 길이와 <_weaponCameraArray> 배열의 길이가 일치하지 않습니다.");
            return;
        }

        _weaponDictionary = new Dictionary<PlayerWeapon, GameObject>();

        for(int i = 0; i < _weaponRenderObjectArray.Length; i++)
        {
            _weaponRenderObjectArray[i].SetActive(false);

            _weaponDictionary.Add((PlayerWeapon)i, _weaponRenderObjectArray[i]);
        }

        InventoryManager.Instance.BindWeaponEvent(OnWeaponCamera);
    }
    #endregion

    private GameObject GetTrinketObject(TrinketItemType type)
    {
        if(_trinketDictionary.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }

        Debug.Log("<TrinketCameraObject>를 반환하지 못했습니다.");
        return null;
    }

    private GameObject GetWeaponObject(PlayerWeapon type)
    {
        if(_weaponDictionary.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }

        Debug.Log("<WeaponCameraObject>를 반환하지 못했습니다.");
        return null;
    }

    private void ResetTrinketCamera()
    {
        foreach(var renderObject in _trinketRenderObjectArray)
        {
            renderObject.SetActive(false);
        }
    }

    private void ResetWeaponCamera()
    {
        foreach(var renderObject in _weaponRenderObjectArray)
        {
            renderObject.SetActive(false);
        }
    }

    private void OnTrinketCamera(TrinketItemType type, bool isActive)
    {
        if (!isActive)
        {
            ResetTrinketCamera();

            return;
        }

        GameObject currentRenderObject = GetTrinketObject(type);

        currentRenderObject.SetActive(true);
    }

    private void OnWeaponCamera(PlayerWeapon weapon, bool isActive)
    {
        if (!isActive)
        {
            ResetWeaponCamera();

            return;
        }

        GameObject currentRenderObject = GetWeaponObject(weapon);

        currentRenderObject.SetActive(true);
    }
}
