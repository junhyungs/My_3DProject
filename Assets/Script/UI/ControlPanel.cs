using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
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

    private List<Tweener> _leftTweeners;
    private List<Tweener> _rightTweeners;

    private int _currentIndex;
    private float _moveDistance = 60f;
    private float _moveDuration = 0.5f;

    private void OnEnable()
    {
        CreateTweeners();

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

        if (isActive)
        {
            AnimationImage(buttons);
        }
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

    private void AnimationImage(PanelButtons button)
    {
        int index = _buttons.IndexOf(button);

        button._leftRectTransform.position = button._initLeftPosition;

        button._rightRectTransform.position = button._initRightPosition;
        
        _leftTweeners[index].Play();

        _rightTweeners[index].Play();
    }

    private void CreateTweeners()
    {
        if(_leftTweeners != null && _rightTweeners != null)
        {
            return;
        }

        _leftTweeners = new List<Tweener>();
        _rightTweeners = new List<Tweener>();

        foreach(var controlButton in _buttons)
        {
            controlButton._initLeftPosition = controlButton._leftRectTransform.position;
            controlButton._initRightPosition = controlButton._rightRectTransform.position;

            float targetX_left = controlButton._leftRectTransform.position.x - _moveDistance;
            float targetX_right = controlButton._rightRectTransform.position.x + _moveDistance;

            _leftTweeners.Add(controlButton._leftRectTransform.DOMoveX(targetX_left, _moveDuration)
                .SetLoops(-1, LoopType.Yoyo).SetUpdate(true));
            _rightTweeners.Add(controlButton._rightRectTransform.DOMoveX(targetX_right, _moveDuration)
                .SetLoops(-1, LoopType.Yoyo).SetUpdate(true));
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
        return;

        _screenOption.SetActive(true);

        gameObject.SetActive(false);
    }
}
