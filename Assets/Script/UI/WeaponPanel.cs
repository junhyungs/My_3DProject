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

    //�κ��丮 �Ŵ������� ��Ȱ��ȭ ������ �� ȣ��.
    public void SetWeaponType(PlayerWeapon weapon, ItemData data)
    {
        for(int i = 0; i < _slotArray.Length; i++)
        {
            if (_slotArray[i].Type == weapon && !_slotArray[i].OnSlot)
            {
                _slotArray[i].OnSlot = true;

                _slotArray[i].Data = data;

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

        var data = slotComponent.Data;

        _descriptionText.text = data.Description;

        _descriptionName.text = data.ItemName;

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
            _currentSlot.InvokeEvent(false);
        }

        _descriptionText.text = string.Empty;

        _descriptionName.text = string.Empty;
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
