using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class BelongingsPanel : MonoBehaviour
{
    [Header("NavigateAction")]
    [SerializeField] private InputActionReference _navigateAction;
    [Header("ButtonObject")]
    [SerializeField] private GameObject[] _buttonObject;
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private void OnEnable()
    {
        _navigateAction.action.Enable();

        _navigateAction.action.performed += SetActiveChildImage;

        EventSystem.current.SetSelectedGameObject(_buttonObject[0]);
    }

    private void SetActiveChildImage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

    private void OnDescription()
    {
        _descriptionText.text = string.Empty;
    }
}
