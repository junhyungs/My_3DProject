using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPanel : MonoBehaviour
{
    [Header("TriggerAction")]
    [SerializeField] private InputActionReference _triggerAction;

    [Header("TriggerButtonComponent")]
    [SerializeField] private TriggerButton[] _buttonComponents;

    [Header("Panels")]
    [SerializeField] private GameObject[] _panels;

    private int _buttonIndex = 0;

    private void OnEnable()
    {
        DisableUI();

        _buttonIndex = 0;

        _buttonComponents[_buttonIndex].OnEnableUI();

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
        foreach(var buttonComponent in _buttonComponents)
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
                    _buttonIndex = _buttonComponents.Length - 1;
                }

                _buttonComponents[_buttonIndex].OnEnableUI();
            }
            else if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                TriggerButtonDisableUI();

                _buttonIndex++;

                if (_buttonIndex >= _buttonComponents.Length)
                {
                    _buttonIndex = 0;
                }

                _buttonComponents[_buttonIndex].OnEnableUI();
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
