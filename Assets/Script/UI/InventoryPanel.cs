using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPanel : MonoBehaviour
{
    [Header("TriggerAction")]
    [SerializeField] private InputActionReference _triggerAction;

    [Header("TriggerButtonComponent")]
    [SerializeField] private List<TriggerButton> _buttonComponent;

    [Header("Panels")]
    [SerializeField] private List<GameObject> _panels;

    private int _buttonIndex = 0;

    private void OnEnable()
    {
        DisableUI();

        _buttonIndex = 0;

        _buttonComponent[_buttonIndex].OnEnableUI();
        
        _triggerAction.action.Enable();

        _triggerAction.action.performed += SelectOnButton;

        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void DisableUI()
    {
        foreach(var panel in _panels)
        {
            panel.gameObject.SetActive(false);
        }

        TriggerButtonDisableUI();
    }

    private void TriggerButtonDisableUI()
    {
        foreach(var buttonComponent in  _buttonComponent)
        {
            buttonComponent.OnDisableUI();
        }
    }

    private void SelectOnButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                TriggerButtonDisableUI();

                _buttonIndex--;

                if(_buttonIndex < 0)
                {
                    _buttonIndex = _buttonComponent.Count - 1;
                }

                _buttonComponent[_buttonIndex].OnEnableUI();
            }
            else if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                TriggerButtonDisableUI();

                _buttonIndex++;

                if (_buttonIndex >= _buttonComponent.Count)
                {
                    _buttonIndex = 0;
                }

                _buttonComponent[_buttonIndex].OnEnableUI();
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
