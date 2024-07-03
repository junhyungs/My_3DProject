using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMoveController : MonoBehaviour
{
    [Header("InputSystem")]
    [SerializeField] private Vector2 m_playerInput;

    [Header("LadderInputSystem")]
    [SerializeField] private Vector2 m_playerLadderInput;

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
    //Roll
    private bool isRoll = false;
    //RollSpeed
    private float m_rollSpeed = 15.0f;
    //Ladder
    private bool isLadder;
    private float m_radderSpeed = 5.0f;
    private bool isLadderDirection = true;
    private Vector3 m_currentPositionY;
    private Vector3 m_previousPositionY;

    public bool IsAction
    {
        get { return isAction; }
        set { isAction = value; }
    }
    public bool IsLadder
    {
        get { return isLadder; }
        set { isLadder = value; }
    }

    public bool IsGround
    {
        get { return isGround; }
        set { isGround = value; }
    }

    private CharacterController m_playerController;
    private Animator m_playerAnimator;

    private void Awake()
    {
        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLadder)
        {
            CheckGround();
            Gravity();
            PlayerMove();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            OnLadder();
        }

    }

    
    //Roll----------------------------------------------------------------------------------------------------------------
    private void OnRoll(InputValue input)
    {
        if (!isAction)
            return; 

        bool isPressed = input.isPressed;

        if (isPressed && !isRoll)
        {
            m_playerAnimator.SetBool("isRoll", true);
        }
    }

    public void StartRoll()
    {
        isAction = false;
        StartCoroutine(Roll());
    }

    public void StopRoll()
    {
        isRoll = false;
        isAction = true;
    }

    private IEnumerator Roll()
    {
        m_playerAnimator.SetBool("isRoll", false);
        isRoll = true;
        
        while (isRoll)
        {
            Vector3 rollPos = (transform.forward).normalized * m_rollSpeed * Time.deltaTime;
            m_playerController.Move(rollPos);
            yield return null;
        }
    }
    //EndRoll---------------------------------------------------------------------------------------------------------------

    //playerAttackMove------------------------------------------------------------------------------------------------------
    public void AnimationStateMove(bool isChargeMax)
    {
        StartCoroutine(AnimationMovement(isChargeMax));
    }

    private IEnumerator AnimationMovement(bool isChargeMax)
    {
        float startTime = Time.time;

        float moveSpeed = isChargeMax ? 10.0f : 3.0f;

        Vector3 direction = transform.forward;

        while(Time.time < startTime + 0.2f)
        {
            m_playerController.Move(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    //EndAttackMove-----------------------------------------------------------------------------------------------------------

    //Movemet-----------------------------------------------------------------------------------------------------------------
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
        return currentHorizontalspeed < m_targetSpeed - m_speedOffSet || currentHorizontalspeed > m_targetSpeed + m_speedOffSet;
    }

    //EndMovement--------------------------------------------------------------------------------------------------------------------

    //Climb--------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            if (isLadder)
            {
                CheckGround();
                m_previousPositionY = transform.position;
                Vector3 move = new Vector3(0f, m_playerInput.y, 0f) * m_radderSpeed * Time.deltaTime;
                m_playerAnimator.SetFloat("ClimbSpeed", m_playerInput.y);
                transform.Translate(move);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            isLadderDirection = m_previousPositionY.y > m_currentPositionY.y ? true : false;
            m_playerAnimator.SetBool("ClimbExit", false);
            transform.SetParent(null);
        }
    }


    private void OnLadder()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);

        Collider[] CheckCollider = Physics.OverlapSphere(spherePosition, 0.5f, LayerMask.GetMask("Ladder"));

        foreach(var checkcoll in CheckCollider)
        {
            if (checkcoll.gameObject.layer == LayerMask.NameToLayer("Ladder"))
            {
                transform.SetParent(checkcoll.gameObject.transform);
                transform.localPosition = Vector3.zero;
                transform.rotation = Quaternion.identity;
                m_playerAnimator.SetTrigger("Climb");
                m_playerAnimator.SetBool("ClimbExit", true);
                m_currentPositionY = transform.position;
                break;
            }
        }   
    }

    public void ClimbStateMove()
    {
        StartCoroutine(ClimbStateMovement());
    }

    private IEnumerator ClimbStateMovement()
    {
        float startTime = Time.time;

        float moveSpeed = 5.0f;

        Vector3 direction = isLadderDirection ? transform.forward : transform.forward * -1f;

        while (Time.time < startTime + 0.2f)
        {
            m_playerController.Move(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    //EndClimb-----------------------------------------------------------------------------------------------------------------

}
