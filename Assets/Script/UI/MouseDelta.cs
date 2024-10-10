using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDelta : MonoBehaviour
{
    [Header("TabAction")]
    [SerializeField] private InputActionReference _tabAction;

    private void OnEnable()
    {
        CurserState(false);

        _tabAction.action.Enable();

        _tabAction.action.performed += TabAction;
    }

    private void OnDisable()
    {
        CurserState(true);
    }

    private void TabAction(InputAction.CallbackContext text)
    {
        if (text.performed)
        {
            gameObject.SetActive(false);
        }
    }

    private void CurserState(bool isLock)
    {
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
