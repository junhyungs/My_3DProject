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
    [Header("TriggerAction")]
    [SerializeField] private InputActionReference _triggerAction;

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
       
        _buttonIndex = 0;

        OnEnableUI(_buttonIndex);

        PerformedAction(true);
    }

    private void OnDisable()
    {
        PerformedAction(false);
    }

    private void PerformedAction(bool onEnable)
    {
        if (onEnable)
        {
            _triggerAction.action.Enable();

            _triggerAction.action.performed += SelectOnButton;

            Time.timeScale = 0f;

            return;
        }

        _triggerAction.action.performed -= SelectOnButton;

        Time.timeScale = 1f;
    }

    private void OnEnableUI(int index)
    {
        _menus[index]._panelUI.SetActive(true);

        SetAlpha(1f, _menus[index].ButtonImage);
    }

    private void OnDisableUI()
    {
        _resetPanels.DisablePanelUI();

        DisablePanelUI();
    }

    private void DisablePanelUI()
    {
        foreach(var menu in _menus)
        {
            menu._panelUI.SetActive(false);

            SetAlpha(0.1f, menu.ButtonImage);
        }
    }

    private void SetAlpha(float alpha, Image currentImage)
    {
        var currentColor = currentImage.color;

        currentColor.a = alpha;

        currentImage.color = currentColor;
    }

    private void SelectOnButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                DisablePanelUI();

                _buttonIndex--;

                if(_buttonIndex < 0)
                {
                    _buttonIndex = _menus.Count - 1;
                }

                OnEnableUI(_buttonIndex);
            }
            else if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                DisablePanelUI();
               
                _buttonIndex++;

                if (_buttonIndex >= _menus.Count)
                {
                    _buttonIndex = 0;
                }

                OnEnableUI(_buttonIndex);
            }
        }
    }

    public void DisableTriggerAction(bool disable)
    {
        if (disable)
        {
            _triggerAction.action.performed -= SelectOnButton;
        }
        else
        {
            _triggerAction.action.performed += SelectOnButton;
        }
    }
}
