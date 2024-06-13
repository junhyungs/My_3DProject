using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
    [Header("PlayerInputValue")]
    [SerializeField] Vector2 m_Input;

    [Header("Roll")]
    [SerializeField] bool isRoll;

    [Header("Look")]
    [SerializeField] bool isLook;

    public Vector2 InputValue
    {
        get { return m_Input; }
    }

    public bool IsRoll
    {
        get { return isRoll; }
    }

    private void OnLook(InputValue input)
    {

    }

    private void OnMove(InputValue input)
    {
        SetMove(input.Get<Vector2>());
    }

    private void SetMove(Vector2 input)
    {
        m_Input = input;
    }
}
