using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("InputValue")]
    [SerializeField] private Vector2 _moveVector2;

    [Header("Roll")]
    [SerializeField] private bool _isRoll;

    [Header("Ladder")]
    [SerializeField] private bool _isLadder;

    #region Property
    public Vector2 InputValue => _moveVector2;  
    public bool IsRoll => _isRoll;
    public bool IsLadder => _isLadder; 
    #endregion

    private void OnMove(InputValue value)
    {
        SetVector2(value.Get<Vector2>());
    }

    public void SetVector2(Vector2 value)
    {
        _moveVector2 = value;
    }

    private void OnRoll(InputValue value)
    {
        var isPressed = value.isPressed;

        SetRoll(isPressed);
    }

    public void SetRoll(bool isPressed)
    {
        _isRoll = isPressed;
    }

    private void OnInteractionLadder(InputValue value)
    {
        var isPressed = value.isPressed;

        SetLadder(isPressed);
    }

    public void SetLadder(bool isLadder)
    {
        _isLadder = isLadder;
    }
}
