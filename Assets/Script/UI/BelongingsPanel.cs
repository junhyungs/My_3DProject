using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class BelongingsPanel : MonoBehaviour
{
    private enum Belongings
    {
        Witch,
        Swampking,
        Betty,
        CrystalHP,
        CrystalMagic
    }

    [Header("NavigateAction")]
    [SerializeField] private InputActionReference _navigateAction;
    [Header("ButtonObject")]
    [SerializeField] private GameObject[] _buttonObject;
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [Header("DescriptionName")]
    [SerializeField] private TextMeshProUGUI _descriptionNameText;

    private Dictionary<GameObject, Belongings> _typeDictionary;

    private void Awake()
    {
        OnAwakeBelongingPanel();
    }

    private void OnAwakeBelongingPanel()
    {
        _typeDictionary = new Dictionary<GameObject, Belongings>();

        for (int i = 0; i < _buttonObject.Length; i++)
        {
            _typeDictionary.Add(_buttonObject[i], (Belongings)i);
        }
    }

    private void OnEnable()
    {
        OnEnableBelongingPanel();
    }

    private void OnEnableBelongingPanel()
    {
        PerformedAction(true);

        EventSystem.current.SetSelectedGameObject(_buttonObject[(int)Belongings.Witch]);

        RefreshDescription(_buttonObject[(int)Belongings.Witch]);
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

            _navigateAction.action.performed += SetActiveChildImage;

            return;
        }

        _navigateAction.action.performed -= SetActiveChildImage;
    }

    private void RefreshDescription(GameObject selectObject)
    {
        ResetText();

        var dataKey = GetBelongings(selectObject);

        if(InventoryManager.Instance.DataDictionary.TryGetValue(dataKey, out ItemData data))
        {
            _descriptionText.text = data.Description;

            _descriptionNameText.text = data.ItemName;

            return;
        }

        ExceptionData(dataKey);
    }

    private void ResetText()
    {
        _descriptionText.text = string.Empty;

        _descriptionNameText.text = string.Empty;
    }

    private string GetBelongings(GameObject selectObject)
    {
        if (_typeDictionary.TryGetValue(selectObject, out Belongings value))
        {
            return value.ToString();
        }

        string newkey = ExceptionBelongings(selectObject);

        return newkey;
    }

    private void SetActiveChildImage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var selectObject = EventSystem.current.currentSelectedGameObject;

            RefreshDescription(selectObject);
        }
    }

    private void ExceptionData(string key)
    {
        var itemData = DataManager.Instance.GetData(key) as ItemData;

        _descriptionText.text = itemData.Description;

        _descriptionNameText.text = itemData.ItemName;
    }

    private string ExceptionBelongings(GameObject selectObject)
    {
        int index = Array.IndexOf(_buttonObject, selectObject);

        _typeDictionary.Add(selectObject, (Belongings)index);

        return _typeDictionary[selectObject].ToString();
    }
}
