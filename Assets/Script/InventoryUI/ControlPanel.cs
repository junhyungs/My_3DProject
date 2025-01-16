using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class PanelButtons
{
    [Header("Button")]
    public Button _button;

    [Header("RightImage/RectTransform")]
    public GameObject _rightImageObject;
    public RectTransform _rightRectTransform;

    [Header("LeftImage/RectTransform")]
    public GameObject _leftImageObject;
    public RectTransform _leftRectTransform;

    [Header("ButtonEvent")]
    public UnityEngine.Events.UnityEvent onClick;

    [HideInInspector]
    public Vector2 _initRightPosition;
    [HideInInspector]
    public Vector2 _initLeftPosition;
}

public class ControlPanel : MonoBehaviour
{
    [Header("NavigateAction")]
    [SerializeField] private InputActionReference _navigateAction;

    [Header("ButtonList")]
    [SerializeField] private List<PanelButtons> _buttons;

    [Header("MouseDelta")]
    [SerializeField] private GameObject _mouseDelta;

    [Header("KeyboardOption")]
    [SerializeField] private GameObject _keyboardOption;

    [Header("ScreenOption")]
    [SerializeField] private GameObject _screenOption;

    private int _currentIndex;

    private void OnEnable()
    {

        ResetControlPanelButton();

        PerformedAction();

        OnControlPanel();
    }

    private void OnDisable()
    {
        _navigateAction.action.performed -= SetNextButton;
    }

    private void OnControlPanel()
    {
        _currentIndex = 0;

        EventSystem.current.SetSelectedGameObject(_buttons[_currentIndex]._button.gameObject);

        SetActiveImage(_buttons[_currentIndex], true);
    }

    private void PerformedAction()
    {
        _navigateAction.action.Enable();

        _navigateAction.action.performed += SetNextButton;
    }

    private void ResetControlPanelButton()
    {
        foreach(var controlButton in _buttons)
        {
            SetActiveImage(controlButton, false);
        }
    }

    private void SetActiveImage(PanelButtons buttons, bool isActive)
    {
        buttons._rightImageObject.SetActive(isActive);

        buttons._leftImageObject.SetActive(isActive);
    }

    private void SetNextButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                ResetControlPanelButton();

                _currentIndex--;

                if(_currentIndex < 0)
                {
                    _currentIndex = _buttons.Count - 1;
                }

                SetActiveImage(_buttons[_currentIndex], true);
            }
            else if(Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                ResetControlPanelButton();

                _currentIndex++; 

                if(_currentIndex >= _buttons.Count)
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

   
    public void OnMouseDelta()
    {
        _mouseDelta.SetActive(true);

        gameObject.SetActive(false);
    }

    public void OnKeyboardOption()
    {
        _keyboardOption.SetActive(true);

        gameObject.SetActive(false);
    }

    public void OnScreenOption()
    {
        _screenOption.SetActive(true);

        gameObject.SetActive(false);
    }
}
