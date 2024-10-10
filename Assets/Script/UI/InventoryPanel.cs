using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{

    [Header("TriggerAction")]
    [SerializeField] private InputActionReference _triggerAction;

    [Header("TriggerButtonComponent")]
    [SerializeField] private List<TriggerButton> _buttonComponent;

    private GameObject _currentButton;
    private int _buttonIndex = 0;

    private void OnEnable()
    {
        Time.timeScale = 0;

        InitializeSelectButton();

        _triggerAction.action.Enable();

        _triggerAction.action.performed += SelectOnButton;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void InitializeSelectButton()
    {
        if (_currentButton == null)
        {
            _buttonComponent[0].OnEnableUI();
        }
        else
        {
            _buttonComponent[_buttonIndex].OnEnableUI();
        }
    }

    private void Test()
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
                Test();

                _buttonIndex--;

                if(_buttonIndex < 0)
                {
                    _buttonIndex = _buttonComponent.Count - 1;
                }

                _buttonComponent[_buttonIndex].OnEnableUI();
            }
            else if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                Test();

                _buttonIndex++;

                if (_buttonIndex >= _buttonComponent.Count)
                {
                    _buttonIndex = 0;
                }

                _buttonComponent[_buttonIndex].OnEnableUI();
            }
        }
    }
}
