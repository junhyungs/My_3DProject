using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
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

    private List<Tweener> _leftTweeners;
    private List<Tweener> _rightTweeners;

    private int _currentIndex;

    private void OnEnable()
    {
        CreateTweeners();

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

        if (isActive)
        {
            AnimationImage(buttons);
        }
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
        if (_leftTweeners != null && _rightTweeners != null)
        {
            return;
        }

        _leftTweeners = new List<Tweener>();
        _rightTweeners = new List<Tweener>();

        foreach (var controlButton in _buttons)
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

    public void OnClickSave_Menu()
    {
        Debug.Log("Save_Menu");
    }

    public void OnClickSave_Exit()
    {
        Debug.Log("Save_Exit");
    }
}
