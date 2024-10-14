using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WeaponPanel : MonoBehaviour
{
    private enum AbilitieType
    {
        Damage,
        Range,
        Critical,
        Speed
    }

    [Header("NavigateAction")]
    [SerializeField] private InputActionReference _navigateAction;

    [Header("SubmitAction")]
    [SerializeField] private InputActionReference _subMitAction;

    [Header("ButtonObject")]
    [SerializeField] private GameObject[] _buttonObject;

    [Header("Slot")]
    [SerializeField] private WeaponSlot[] _slotArray;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("DescriptionName")]
    [SerializeField] private TextMeshProUGUI _descriptionName;

    [Header("AbilitieDescription")]
    [SerializeField] private TextMeshProUGUI[] _abilitieText;

    private Dictionary<GameObject, WeaponSlot> _slotDictionary;

    private WeaponSlot _currentSlot;

    private void Awake()
    {
        OnAwakeWeaponPanel();
    }

    private void OnAwakeWeaponPanel()
    {
        _slotDictionary = new Dictionary<GameObject, WeaponSlot>();

        for(int i = 0; i < _buttonObject.Length; i++)
        {
            GameObject slotObject = _buttonObject[i];

            WeaponSlot slotComponent = slotObject.GetComponent<WeaponSlot>();

            _slotDictionary.Add(slotObject, slotComponent);
        }
    }

    private void OnEnable()
    {
        OnEnableWeaponPanel();
    }

    private void OnEnableWeaponPanel()
    {
        PerformedAction(true);

        EventSystem.current.SetSelectedGameObject(_buttonObject[0]);

        RefreshDescription(_buttonObject[0]);
    }

    private void OnDisable()
    {
        PerformedAction(false);

        if(_currentSlot != null)
        {
            _currentSlot.CaptureImage();

            _currentSlot.InvokeEvent(false);
        }
    }

    private void PerformedAction(bool onEnable)
    {
        if (onEnable)
        {
            _navigateAction.action.Enable();

            _subMitAction.action.Enable();

            _subMitAction.action.performed += SetActiveChildImage;

            _navigateAction.action.performed += SetActiveChildImage;

            return;
        }

        _subMitAction.action.performed -= SetActiveChildImage;

        _navigateAction.action.performed -= SetActiveChildImage;
    }

    //인벤토리 매니저에서 비활성화 상태일 때 호출.
    public void SetWeaponType(PlayerWeapon weapon, ItemData data, PlayerWeaponData weaponData)
    {
        for(int i = 0; i < _slotArray.Length; i++)
        {
            if (_slotArray[i].Type == weapon && !_slotArray[i].OnSlot)
            {
                _slotArray[i].OnSlot = true;

                _slotArray[i].Data = data;

                _slotArray[i].WeaponData = weaponData;  

                return;
            }
        }
    }

    private void RefreshDescription(GameObject selectObject)
    {
        ResetText();
        
        var slotComponent = GetWeaponSlot(selectObject);

        if (!slotComponent.OnSlot)
        {
            return;
        }

        slotComponent.LiveImage();

        var data = slotComponent.Data;

        _descriptionText.text = data.Description;

        _descriptionName.text = data.ItemName;

        var weaponData = slotComponent.WeaponData;

        _abilitieText[(int)AbilitieType.Damage].text = weaponData.Power.ToString() + "X";

        _abilitieText[(int)AbilitieType.Range].text = weaponData.NormalEffectRange.ToString() + "X";

        _abilitieText[(int)AbilitieType.Critical].text = weaponData.ChargePower.ToString() + "X";

        _abilitieText[(int)AbilitieType.Speed].text = 1f.ToString() + "X";    

        _currentSlot = slotComponent;

        slotComponent.InvokeEvent(true);
    }

    private WeaponSlot GetWeaponSlot(GameObject selectObject)
    {
        if(_slotDictionary.TryGetValue(selectObject, out WeaponSlot slot))
        {
            return slot;
        }

        return null;
    }

    private void ResetText()
    {
        if(_currentSlot != null)
        {
            _currentSlot.CaptureImage();

            _currentSlot.InvokeEvent(false);
        }

        _descriptionText.text = string.Empty;

        _descriptionName.text = string.Empty;

        foreach(var abilitieDescription in _abilitieText)
        {
            abilitieDescription.text = string.Empty;    
        }
    }

    private void SetActiveChildImage(InputAction.CallbackContext context)
    {
        var selectObject = EventSystem.current.currentSelectedGameObject;

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            var slot = GetWeaponSlot(selectObject);

            if (slot.OnSlot)
            {
                WeaponManager.Instance.ChangeWeapon(slot.Type);
            }
        }

        RefreshDescription(selectObject);
    }

}
