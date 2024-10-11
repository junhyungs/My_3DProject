using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDelta : MonoBehaviour
{
    [Header("BackAction")]
    [SerializeField] private InputActionReference _backAction;

    [Header("ControlPanel")]
    [SerializeField] private GameObject _controlPanel;

    [Header("InventoryPanel")]
    [SerializeField] private InventoryPanel _inventroyPanel;

    [Header("BottomUI")]
    [SerializeField] private TextMeshProUGUI _bottomText;

    private void OnEnable()
    {
        CurserState(false);
        ChangeBottomText(true);

        _backAction.action.Enable();

        _backAction.action.performed += BackAction;

        _inventroyPanel.DisableTriggerAction(true);
    }

    private void OnDisable()
    {
        CurserState(true);
        ChangeBottomText(false);

        _backAction.action.performed -= BackAction; 

        _backAction.action.Disable();

        _inventroyPanel.DisableTriggerAction(false);
    }

    private void BackAction(InputAction.CallbackContext text)
    {
        if (text.performed)
        {
            gameObject.SetActive(false);

            _controlPanel.SetActive(true);  
        }
    }

    private void CurserState(bool isLock)
    {
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void ChangeBottomText(bool chage)
    {
        _bottomText.text = chage ? "[Q] 뒤로" : "[Tab] 뒤로";
    }
}
