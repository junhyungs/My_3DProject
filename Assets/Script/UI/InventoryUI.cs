using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("CancelAction")]
    [SerializeField] private InputActionReference _cancelAction;
    [Header("PlayerAttackAction")]
    [SerializeField] private InputActionAsset _playerActionAsset;
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
        _playerActionAsset.Disable();

        _inventoryPanel.SetActive(true);

        _playerUI.MovePlayerUI(true);
    }

    private void OnDisableInventoryUI()
    {
        _playerActionAsset.Enable();

        _inventoryPanel.SetActive(false);

        _playerUI.MovePlayerUI(false);
    }
}
