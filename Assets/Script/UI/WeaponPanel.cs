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

    [Header("ButtonObject")]
    [SerializeField] private GameObject[] _buttonObject;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("DescriptionName")]
    [SerializeField] private TextMeshProUGUI _descriptionName;

    [Header("AbilitieValue")]
    [SerializeField] private TextMeshProUGUI[] _abilitieText;

    private Dictionary<GameObject, PlayerWeapon> _typeDictionary;
    private Dictionary<TextMeshProUGUI, AbilitieType> _textTypeDictionary;

    private void Awake()
    {
        OnAwakeWeaponPanel();
    }

    private void OnAwakeWeaponPanel()
    {
        _typeDictionary = new Dictionary<GameObject, PlayerWeapon>();

        _textTypeDictionary = new Dictionary<TextMeshProUGUI, AbilitieType>();

        for(int i = 0; i <  _buttonObject.Length; i++)
        {
            _typeDictionary.Add(_buttonObject[i], (PlayerWeapon)i);
        }

        for(int i = 0; i < _abilitieText.Length; i++)
        {
            _textTypeDictionary.Add(_abilitieText[i], (AbilitieType)i);
        }
    }

    private void OnEnable()
    {
        OnEnableWeaponPanel();
    }

    private void OnEnableWeaponPanel()
    {
        PerformedAction(true);

        EventSystem.current.SetSelectedGameObject(_buttonObject[(int)PlayerWeapon.Sword]);

        RefreshDescription(_buttonObject[(int)PlayerWeapon.Sword]);
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
        //TODO 서브밋 액션을 넣어서 해당 버튼이 눌리면 웨폰 매니저의 ChangeWeapon 호출하면 무기 바뀜.
        //무기 아이템을 얻으면 아이템 매니저의 SetWeapon 호출해서 무기 타입 넣어줘야함.
        bool isWeapon = IsWeapon(selectObject);

        if (!isWeapon)
        {
            return;
        }

        var dataKey = GetWeapons(selectObject);

        if(InventoryManager.Instance.DataDictionary.TryGetValue(dataKey, out ItemData data))
        {
            _descriptionText.text = data.Description;

            _descriptionName.text = data.ItemName;

            return;
        }

        ExceptionData(dataKey);
    }

    private string GetWeapons(GameObject selectObject)
    {
        if(_typeDictionary.TryGetValue(selectObject, out PlayerWeapon value))
        {
            return value.ToString();    
        }

        var newKey = ExceptionWeapon(selectObject);

        return newKey.ToString();
    }

    private bool IsWeapon(GameObject selectObject)
    {
        var weapon = _typeDictionary[selectObject];

        if (InventoryManager.Instance.WeaponSet.Contains(weapon))
        {
            return true;
        }

        return false;
    }

    private void ResetText()
    {
        _descriptionText.text = string.Empty;

        _descriptionName.text = string.Empty;

        foreach(var abilitieText in _abilitieText)
        {
            abilitieText.text = string.Empty;
        }
    }

    private void SetActiveChildImage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var selectObject = EventSystem.current.currentSelectedGameObject;

            RefreshDescription(selectObject);
        }
    }

    private PlayerWeapon ExceptionWeapon(GameObject selectObject)
    {
        int index = Array.IndexOf(_buttonObject, selectObject);

        _typeDictionary.Add(selectObject, (PlayerWeapon)index);

        return _typeDictionary[selectObject];
    }

    private void ExceptionData(string key)
    {
        var itemData = DataManager.Instance.GetData(key) as ItemData;

        _descriptionText.text = itemData.Description;

        _descriptionName.text = itemData.ItemName;
    }
}
