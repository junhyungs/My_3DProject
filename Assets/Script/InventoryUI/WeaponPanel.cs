using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("DescriptionName")]
    [SerializeField] private TextMeshProUGUI _descriptionName;

    [Header("AbilitieDescription")]
    [SerializeField] private TextMeshProUGUI[] _abilitieText;

    private Dictionary<GameObject, WeaponSlot> _slotDictionary;
    private WeaponSlot _currentSlot;

    private void OnInitializeWeaponPanel()
    {
        if(_slotDictionary != null)
        {
            return;
        }

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
        OnInitializeWeaponPanel();

        OnEnableInputAction();

        EventSystem.current.SetSelectedGameObject(_buttonObject[0]);

        RefreshDescription(_buttonObject[0]);
    }

    private void OnDisable()
    {
        OnDisableInputAction();

        if(_currentSlot != null)
        {
            _currentSlot.CaptureImage();

            _currentSlot.InvokeEvent(false);
        }
    }

    private void OnEnableInputAction()
    {
        _navigateAction.action.Enable();
        _subMitAction.action.Enable();

        _subMitAction.action.performed += SetActiveChildImage;
        _navigateAction.action.performed += SetActiveChildImage;
    }

    private void OnDisableInputAction()
    {
        _subMitAction.action.performed -= SetActiveChildImage;
        _navigateAction.action.performed -= SetActiveChildImage;
    }

    //인벤토리 매니저에서 비활성화 상태일 때 호출.
    public void SetWeaponType(PlayerWeapon weapon, ItemData data, PlayerWeaponData weaponData)
    {
        OnInitializeWeaponPanel();

        foreach (var slot in _slotDictionary.Values)
        {
            if(slot.Type == weapon)
            {
                slot.InitializeWeaponSlot();

                slot.Data = data;

                slot.WeaponData = weaponData;
            }
        }
    }

    private void RefreshDescription(GameObject selectObject)
    {
        ResetText();
        
        var slotComponent = GetWeaponSlot(selectObject);

        if (slotComponent.Data == null)
        {
            return;
        }

        OnDescription(slotComponent);

        OnAbilityDescription(slotComponent);

        slotComponent.LiveImage();

        slotComponent.InvokeEvent(true);

        _currentSlot = slotComponent;
    }

    private void OnDescription(WeaponSlot slot)
    {
        var data = slot.Data;

        _descriptionText.text = data.Description;

        _descriptionName.text = data.ItemName;
    }

    private void OnAbilityDescription(WeaponSlot slot)
    {
        var weaponData = slot.WeaponData;

        _abilitieText[(int)AbilitieType.Damage].text = weaponData.Power.ToString() + "X";

        _abilitieText[(int)AbilitieType.Range].text = weaponData.NormalEffectRange.ToString() + "X";

        _abilitieText[(int)AbilitieType.Critical].text = weaponData.ChargePower.ToString() + "X";

        _abilitieText[(int)AbilitieType.Speed].text = 1f.ToString() + "X";
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

        RefreshDescription(selectObject);
    }

}
