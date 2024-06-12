using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float m_walkSpeed;

    [Header("ChangeSpeedValue")]
    [SerializeField] private float m_changeSpeedValue;

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
    //Gravity
    private float m_gravity = -9.81f;


    private PlayerInputSystem m_playerInput;
    private CharacterController m_playerController;
    private Animator m_playerAnimator;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInputSystem>();
        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckGround();
        PlayerMove();
    }

    private void PlayerMove()
    {
        m_targetSpeed = m_walkSpeed;

        if(m_playerInput.InputValue == Vector2.zero)
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

        PlayerRotation();

        Vector3 inputDirection = transform.TransformDirection(new Vector3(m_playerInput.InputValue.x, 0f, m_playerInput.InputValue.y));

        m_playerController.Move(inputDirection * m_mySpeed * Time.deltaTime);

        
    }

    private void PlayerRotation()
    {
        Ray mouseRayposition = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(mouseRayposition, out RaycastHit hit, 100))
        {
            Vector3 playerLookrotation = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            float distance = Vector3.Distance(transform.position, hit.point);

            if(distance > 0.1f)
            {
                transform.LookAt(playerLookrotation);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 GizmoPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Gizmos.DrawWireSphere(GizmoPosition, 0.5f);
    }

    private void CheckGround()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        isGround = Physics.CheckSphere(spherePosition, 0.5f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

        if (isGround)
        {
            Debug.Log("¶¥");
        }
        else
        {
            Debug.Log("¶¥¾Æ´Ô");
        }
    }

    private bool SpeedCorrection(float currentHorizontalspeed)
    {
        return m_currentHorizontalSpeed < m_targetSpeed - m_speedOffSet || m_currentHorizontalSpeed > m_targetSpeed + m_speedOffSet;
    }
}
