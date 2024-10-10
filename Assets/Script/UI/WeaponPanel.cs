using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WeaponPanel : MonoBehaviour
{
    private enum AbilitieValue
    {
        Damage,
        Range,
        Critical,
        Speed
    }

    [Header("NavigateAction")]
    [SerializeField] private InputActionReference _navigateAction;
    [Header("ButtonObject")]
    [SerializeField] private GameObject[] _buttonObject;
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [Header("AbilitieValue")]
    [SerializeField] private TextMeshProUGUI[] _abilitieText; 



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
