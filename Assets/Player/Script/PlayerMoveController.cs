using System.Collections;
using System.Collections.Generic;

using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    [Header("InputSystem")]
    [SerializeField] private Vector2 m_playerInput;

    [Header("Speed")]
    [SerializeField] private float m_walkSpeed;

    [Header("ChangeSpeedValue")]
    [SerializeField] private float m_changeSpeedValue;

    [Header("Gravity")]
    [SerializeField] private float m_gravity;

    //Player CharacterControllerVelocity
    private float m_currentHorizontalSpeed;
    //Player TargetSpeed
    private float m_targetSpeed;
    //Player Speed
    private float m_mySpeed;
    //SpeedOffSet
    private float m_speedOffSet = 0.1f;
    //GroundCheck
    private bool isGround;
    //VerticalVelocity
    private float m_verticalVelocity;
    //RotationVelocity
    private float m_rotationVelocity;
    //PlayerMoveControll
    private bool isAction = true;

    private CharacterController m_playerController;
    private Animator m_playerAnimator;

    private void Awake()
    {
        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckGround();
        Gravity();
        PlayerMove();
    }

    private void OnMove(InputValue input)
    {
        SetMove(input.Get<Vector2>());
    }

    private void SetMove(Vector2 input)
    {
        m_playerInput = input;
    }

    private void PlayerMove()
    {
        if (isAction)
        {
            m_targetSpeed = m_walkSpeed;

            if (m_playerInput == Vector2.zero)
            {
                m_targetSpeed = 0f;
            }

            m_currentHorizontalSpeed = new Vector3(m_playerController.velocity.x, 0f, m_playerController.velocity.z).magnitude;

            bool correction = SpeedCorrection(m_currentHorizontalSpeed);

            if (correction)
            {
                m_mySpeed = Mathf.Lerp(m_currentHorizontalSpeed, m_targetSpeed, m_changeSpeedValue * Time.deltaTime);

                m_mySpeed = Mathf.Round(m_mySpeed * 1000f) / 1000f;
            }
            else
                m_mySpeed = m_targetSpeed;

            Vector3 rotationDir = new Vector3(m_playerInput.x, 0, m_playerInput.y).normalized;

            if (m_playerInput != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(rotationDir.x, rotationDir.z) * Mathf.Rad2Deg;

                float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref m_rotationVelocity, 0.12f);

                transform.rotation = Quaternion.Euler(0, smoothRotation, 0);
            }

            m_playerAnimator.SetFloat("MoveSpeed", m_mySpeed);
            m_playerController.Move((rotationDir * m_mySpeed + Vector3.up * m_verticalVelocity) * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 GizmoPosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        Gizmos.DrawWireSphere(GizmoPosition, 0.5f);
    }

    private void Gravity()
    {
        if (isGround)
        {
            if (m_verticalVelocity < 0)
            {
                m_verticalVelocity = -2f;
            }
        }
        else
        {
            m_verticalVelocity += m_gravity * Time.deltaTime;
        }
    }

    private void CheckGround()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        isGround = Physics.CheckSphere(spherePosition, 0.5f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

        if (isGround)
        {
            m_playerAnimator.SetBool("isGround", false);
        }
        else
        {
            m_playerAnimator.SetBool("isGround", true);
        }
    }

    private bool SpeedCorrection(float currentHorizontalspeed)
    {
        return m_currentHorizontalSpeed < m_targetSpeed - m_speedOffSet || m_currentHorizontalSpeed > m_targetSpeed + m_speedOffSet;
    }
}
