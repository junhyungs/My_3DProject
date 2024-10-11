using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardOption : MonoBehaviour
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
        ChangeBottomText(true);

        _backAction.action.Enable();

        _backAction.action.performed += BackAction;

        _inventroyPanel.DisableTriggerAction(true);
    }

    private void OnDisable()
    {
        ChangeBottomText(false);

        _backAction.action.performed -= BackAction;

        _backAction.action.Disable();

        _inventroyPanel.DisableTriggerAction(false);
    }

    private void BackAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gameObject.SetActive(false);

            _controlPanel.SetActive(true);
        }
    }

    private void ChangeBottomText(bool chage)
    {
        _bottomText.text = chage ? "[Q] 뒤로" : "[Tab] 뒤로";
    }
}
