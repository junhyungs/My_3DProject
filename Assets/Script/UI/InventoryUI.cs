using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("CancelAction")]
    [SerializeField] private InputActionReference _cancelAction;

    [Header("PlayerUI")]
    [SerializeField] private PlayerUI _playerUI;

    private GameObject _inventoryPanel;

    private void Awake()
    {
        _inventoryPanel = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        _cancelAction.action.Enable();

        _cancelAction.action.performed += OnCancel;
    }

    private void OnDisable()
    {
        _cancelAction.action.performed -= OnCancel;

        _cancelAction.action.Disable();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Action enableAction = _inventoryPanel.activeSelf ? OnDisableInventoryUI : OnEnableInventoryUI;

            enableAction.Invoke();
        }
    }

    private void OnEnableInventoryUI()
    {
        GameManager.Instance.PlayerLock(true);

        _inventoryPanel.SetActive(true);

        _playerUI.MovePlayerUI(true);
    }

    private void OnDisableInventoryUI()
    {
        GameManager.Instance.PlayerLock(false);

        _inventoryPanel.SetActive(false);

        _playerUI.MovePlayerUI(false);
    }

    public void ActionControl(bool control)
    {
        if (control)
        {
            _cancelAction.action.performed -= OnCancel;

            return;
        }

        _cancelAction.action.performed += OnCancel; 
    }
}
