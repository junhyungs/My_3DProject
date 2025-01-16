using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class AbilityPanel : MonoBehaviour
{
    private enum AbilityButton
    {
        Power,
        Agility,
        Speed,
        Magic
    }

    [Header("SubmitNavigateAction")]
    [SerializeField] private InputActionReference _submitNavigateAction;

    [Header("TabAction")]
    [SerializeField] private InputActionReference _tabAction;

    [Header("PlayerUI")]
    [SerializeField] private PlayerUI _playerUI;

    [Header("Button")]
    [SerializeField] private GameObject[] _buttons;

    [Header("DescriptionName")]
    [SerializeField] private TextMeshProUGUI _descriptionNameText;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("Price")]
    [SerializeField] private TextMeshProUGUI _priceText;

    private Dictionary<GameObject, AbilityButton> _typeDictionary;

    public List<AbilityData> DataList { get; set; }

    private void Awake()
    {
        _typeDictionary = new Dictionary<GameObject, AbilityButton>();

        for(int i = 0; i < _buttons.Length; i++)
        {
            _typeDictionary.Add(_buttons[i], (AbilityButton)i);
        }
    }

    private void OnEnable()
    {
        BindAction(true);

        EventSystem.current.SetSelectedGameObject(_buttons[(int)AbilityButton.Power]);

        RefreshDescription(_buttons[(int)AbilityButton.Power]);
    }

    private void BindAction(bool onEnable)
    {
        _playerUI.MovePlayerUI(onEnable);

        InventoryManager.Instance.OnInventoryTabAction(onEnable);

        if (onEnable)
        {
            _submitNavigateAction.action.Enable();

            _tabAction.action.Enable();

            _submitNavigateAction.action.performed += AbilityUIControl;

            _tabAction.action.performed += AbilityUIActive;
        }
        else
        {
            GameManager.Instance.PlayerLock(onEnable);

            _submitNavigateAction.action.performed -= AbilityUIControl;

            _tabAction.action.performed -= AbilityUIActive;
        }
    }

    private void OnDisable()
    {
        BindAction(false);
    }

    private void AbilityUIControl(InputAction.CallbackContext context)
    {
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;

        RefreshDescription(selectObject);
    }

    private void AbilityUIActive(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }

    private void RefreshDescription(GameObject selectObject)
    {
        ResetText();

        var buttonType = GetButton(selectObject);

        var data = DataList[(int)buttonType];

        _descriptionNameText.text = data.DescriptionName;

        _descriptionText.text = data.Description;

        _priceText.text = "X " + data.PriceList[0].ToString();
    }

    private AbilityButton GetButton(GameObject selectObject)
    {
        if (_typeDictionary.ContainsKey(selectObject))
        {
            return _typeDictionary[selectObject];
        }

        int index = Array.IndexOf(_buttons, selectObject);

        _typeDictionary.Add(selectObject, (AbilityButton)index);

        return _typeDictionary[selectObject];
    }

    private void ResetText()
    {
        _descriptionText.text = string.Empty;

        _descriptionNameText.text = string.Empty;
    }
}

