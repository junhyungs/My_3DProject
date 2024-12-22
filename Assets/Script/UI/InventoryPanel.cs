using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class MenuUI
{
    [Header("TriggerButton")]
    public GameObject _triggerButton;

    [Header("BindPanel")]
    public GameObject _panelUI;

    public Image ButtonImage { get; set; }
}

public class InventoryPanel : MonoBehaviour
{
    [Header("TriggerButtonActionX")]
    [SerializeField] private InputActionReference _triggerButtonX;

    [Header("TriggerButtonActionZ")]
    [SerializeField] private InputActionReference _triggerButtonZ;

    [Header("MenuUI")]
    [SerializeField] private List<MenuUI> _menus;

    private ResetInventoryUI _resetPanels;

    private int _buttonIndex = 0;

    private void Awake()
    {
        foreach(var menu in _menus)
        {
            var imageComponent = menu._triggerButton.GetComponent<Image>();

            if (imageComponent != null)
            {
                menu.ButtonImage = imageComponent;
            }
        }

        _resetPanels = GetComponent<ResetInventoryUI>();
    }

    private void OnEnable()
    {
        OnDisableUI();

        OnEnableUI(_buttonIndex);

        OnEnableInputAction();

        SetTimeScale(true);
    }

    private void OnDisable()
    {
        OnDisableInputAction();

        SetTimeScale(false);
    }

    private void OnEnableInputAction()
    {
        _triggerButtonX.action.Enable();
        _triggerButtonZ.action.Enable();

        _triggerButtonX.action.performed += SelectMenuUI_X;
        _triggerButtonZ.action.performed += SelectMenuUI_Z;        
    }

    private void OnDisableInputAction()
    {
        _triggerButtonX.action.performed -= SelectMenuUI_X;
        _triggerButtonZ.action.performed -= SelectMenuUI_Z;

        _triggerButtonX.action.Disable();
        _triggerButtonZ.action.Disable();
    }

    private void SetTimeScale(bool onEnable)
    {
        Time.timeScale = onEnable ? 0f : 1f;
    }

    private void OnEnableUI(int index)
    {
        SetUIState(index, true, 1f);
    }

    private void OnDisableUI()
    {
        _buttonIndex = 0;

        _resetPanels.DisablePanelUI();

        DisableMenuUI();
    }

    private void DisableMenuUI()
    {
        for(int i = 0; i < _menus.Count; i++)
        {
            SetUIState(i, false, 0.1f);
        }
    }

    private void SetUIState(int index, bool isActive, float alpha)
    {
        _menus[index]._panelUI.SetActive(isActive);

        SetAlpha(alpha, _menus[index].ButtonImage);
    }

    private void SetAlpha(float alpha, Image currentImage)
    {
        var currentColor = currentImage.color;

        currentColor.a = alpha;

        currentImage.color = currentColor;
    }

    private void SelectMenuUI_X(InputAction.CallbackContext context)
    {
        MenuDirection(1);
    }

    private void SelectMenuUI_Z(InputAction.CallbackContext context)
    {
        MenuDirection(-1);
    }

    private void MenuDirection(int index)
    {
        DisableMenuUI();

        _buttonIndex += index;

        if (_buttonIndex < 0)
            _buttonIndex = _menus.Count - 1;

        if (_buttonIndex >= _menus.Count)
            _buttonIndex = 0;

        OnEnableUI(_buttonIndex);
    }

    public void DisableTriggerAction(bool disable)
    {
        if (disable)
        {
            OnDisableInputAction();
        }
        else
        {
            OnEnableInputAction();
        }
    }
}
