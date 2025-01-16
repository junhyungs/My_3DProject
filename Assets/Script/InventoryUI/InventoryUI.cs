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

        _cancelAction.action.performed += OnCancelInventoryUI;
    }

    private void OnDisable()
    {
        _cancelAction.action.performed -= OnCancelInventoryUI;

        _cancelAction.action.Disable();
    }

    private void OnCancelInventoryUI(InputAction.CallbackContext context)
    {
        if (_inventoryPanel.activeSelf)
        {
            InventroyUI(false);
        }
        else
        {
            InventroyUI(true);
        }
    }

    private void InventroyUI(bool active)
    {
        GameManager.Instance.PlayerLock(active);

        _inventoryPanel.SetActive(active);

        _playerUI.MovePlayerUI(active);
    }

    public void ActionControl(bool control)
    {
        if (control)
        {
            _cancelAction.action.performed -= OnCancelInventoryUI;

            return;
        }

        _cancelAction.action.performed += OnCancelInventoryUI; 
    }
}
