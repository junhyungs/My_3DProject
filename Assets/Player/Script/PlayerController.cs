using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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



    private PlayerInputSystem m_playerInput;
    private CharacterController m_playerController;
    private PlayerAttackController m_playerAttack;
    private CinemachineCamera m_playerCam;
    private Animator m_playerAnimator;

    private void Awake()
    {
        m_playerAttack = GetComponent<PlayerAttackController>();
        m_playerInput = GetComponent<PlayerInputSystem>();
        m_playerCam = GetComponent<CinemachineCamera>();
        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckGround();
        Gravity();
        PlayerClickRotation();
        PlayerMove();
    }

    private void PlayerMove()
    {
        if (isAction)
        {
            m_targetSpeed = m_walkSpeed;

            if (m_playerInput.InputValue == Vector2.zero)
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

            Vector3 rotationDir = new Vector3(m_playerInput.InputValue.x, 0, m_playerInput.InputValue.y).normalized;

            if (m_playerInput.InputValue != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(rotationDir.x, rotationDir.z) * Mathf.Rad2Deg;

                float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref m_rotationVelocity, 0.12f);

                transform.rotation = Quaternion.Euler(0, smoothRotation, 0);
            }

            m_playerController.Move((rotationDir * m_mySpeed + Vector3.up * m_verticalVelocity) * Time.deltaTime);

        }
    }


    //private bool PlayerClickRotation()
    //{
    //    if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
    //    {
    //        Ray mouseRayposition = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        if (Physics.Raycast(mouseRayposition, out RaycastHit hit, 100))
    //        {
    //            Vector3 playerLookrotation = new Vector3(hit.point.x, transform.position.y, hit.point.z);

    //            float distance = Vector3.Distance(transform.position, hit.point);

    //            if (distance > 0.1f)
    //            {
    //                transform.LookAt(playerLookrotation);
                    
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}

    private void PlayerClickRotation()
    {
        bool GetMouseButton = Input.GetMouseButton(1);
        bool GetMouseButtonDown = Input.GetMouseButtonDown(0);

        if (GetMouseButton || GetMouseButtonDown)
        {
            

            if (GetMouseButtonDown)
            {
                isAction = false;
            }
            else if (GetMouseButton)
            {
                isAction = false;
            }

            Ray mouseRayposition = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRayposition, out RaycastHit hit, 100))
            {
                Vector3 playerLookrotation = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                float distance = Vector3.Distance(transform.position, hit.point);

                if (distance > 0.1f)
                {
                    transform.LookAt(playerLookrotation);
                }
            }
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
