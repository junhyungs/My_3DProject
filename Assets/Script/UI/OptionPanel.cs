using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class OptionPanel : MonoBehaviour
{
    [Header("NavigateAction")]
    [SerializeField] private InputActionReference _navigateAction;

    [Header("ButtonList")]
    [SerializeField] private List<PanelButtons> _buttons;

    [Header("MoveDistance")]
    [SerializeField] private float _moveDistance;

    [Header("MoveDuration")]
    [SerializeField] private float _moveDuration;

    private int _currentIndex;

    private void OnEnable()
    {
        ResetOptionPanelButton();

        PerformedAction();

        OnOptionPanel();
    }

    private void OnDisable()
    {
        _navigateAction.action.performed -= InvokeButton;
    }

    private void OnOptionPanel()
    {
        _currentIndex = 0;

        EventSystem.current.SetSelectedGameObject(_buttons[_currentIndex]._button.gameObject);

        SetActiveImage(_buttons[_currentIndex], true);
    }

    private void PerformedAction()
    {
        _navigateAction.action.Enable();

        _navigateAction.action.performed += InvokeButton;
    }

    private void SetActiveImage(PanelButtons buttons, bool isActive)
    {
        buttons._rightImageObject.SetActive(isActive);

        buttons._leftImageObject.SetActive(isActive);
    }

    private void ResetOptionPanelButton()
    {
        foreach (var controlButton in _buttons)
        {
            SetActiveImage(controlButton, false);
        }
    }

    private void InvokeButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                ResetOptionPanelButton();

                _currentIndex--;

                if (_currentIndex < 0)
                {
                    _currentIndex = _buttons.Count - 1;
                }

                SetActiveImage(_buttons[_currentIndex], true);
            }
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                ResetOptionPanelButton();

                _currentIndex++;

                if (_currentIndex >= _buttons.Count)
                {
                    _currentIndex = 0;
                }

                SetActiveImage(_buttons[_currentIndex], true);
            }
            else if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                _buttons[_currentIndex].onClick?.Invoke();
            }
        }
    }
   
    public void OnClickSave_Menu()
    {
        Debug.Log("Save_Menu");
    }

    public void OnClickSave_Exit()
    {
        Debug.Log("Save_Exit");
    }
}
