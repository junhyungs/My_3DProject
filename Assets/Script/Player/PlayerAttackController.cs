using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttackController : MonoBehaviour
{
    [Header("ProjectilePositionObject")]
    [SerializeField] private GameObject m_arrowPositionObject;
    [SerializeField] private GameObject m_firePositionObject;
    [SerializeField] private GameObject m_hookPositionObject;
    [SerializeField] private GameObject m_bombPositionObject;

    private GameObject[] m_PositionObject;
    private PlayerWeaponController m_weaponController;
    private PlayerSkillController m_skillController;
    private PlayerPlane _plane;
    private Animator m_attackAnimation;

    public GameObject[] PositionObject => m_PositionObject;

    private LayerMask _mouseTargetLayer;
    private bool chargeMax;
    private bool chargeAttackDirection = true;
    private bool isAction = true;

    public bool IsAction
    {
        get { return isAction;}
        set { isAction = value;}    
    }
    public bool IsChargeMax
    {
        get { return chargeMax; }
        set { chargeMax = value; }
    }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        OnUpdate();
    }

    private void Init()
    {
        m_weaponController = GetComponent<PlayerWeaponController>();   
        m_skillController = GetComponent<PlayerSkillController>();  
        m_attackAnimation = GetComponent<Animator>();
        _plane = transform.GetComponentInChildren<PlayerPlane>();

        m_PositionObject = new GameObject[3];

        m_PositionObject[0] = m_arrowPositionObject;
        m_PositionObject[1] = m_firePositionObject;
        m_PositionObject[2] = m_bombPositionObject;

        _mouseTargetLayer = LayerMask.GetMask("Ground");
    }

    private void OnUpdate()
    {
        if (!isAction)
            return;

        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            LookAtMouse();
        }

        OnSkillChange();
    }

    private void OnLeftClickAttack(InputValue input)
    {
        if (!isAction)
            return;

        bool isPressed = input.isPressed;

        LeftClick(isPressed);
    }

    private void LeftClick(bool isPressed)
    {
        if (isPressed)
        {
            m_weaponController.Attack();
            chargeAttackDirection = !chargeAttackDirection;
            LookAtMouse();
        }
    }

    private void OnRightClickAttack(InputValue input)
    {
        if (!isAction)
            return;

        bool isPressed = input.isPressed;

        RightClick(isPressed);
    }

    private void RightClick(bool isPressed)
    {
        PlayerSkill skillType = m_skillController.SkillType;


        if (skillType != PlayerSkill.Hook)
        {
            m_skillController.ActiveSkillObject(isPressed);

            m_skillController.CurrentSkillAnimation(isPressed);

            if (!isPressed)
            {
                OnSkill(skillType);
            }
        }
        else
        {
            m_skillController.ActiveSkillObject(isPressed);

            if (isPressed)
            {
                m_skillController.CurrentSkillAnimation(isPressed);
            }
        }
        
    }

    private void OnChargeAttack(InputValue input)
    {
        if (!isAction)
            return;

        bool isPressed = input.isPressed;

        MiddleClick(isPressed);
    }

    private void MiddleClick(bool isPressed)
    {
        if (isPressed)
        {
            ChargeAttackAnimation();
        }
        else
        {
            ChargeAttack();
        }

    }
    private void ChargeAttack()
    {
        if (chargeMax)
        {
            m_attackAnimation.SetBool("ChargeAttack", false);
            m_weaponController.Attack();
            chargeAttackDirection = !chargeAttackDirection;
        }
        else
        {
            m_weaponController.ChargeAttackReset();
        }
    }

    private void ChargeAttackAnimation()
    {
        if (chargeAttackDirection)
        {
            m_attackAnimation.SetTrigger("ChargeAttackR");
        }
        else
        {
            m_attackAnimation.SetTrigger("ChargeAttackL");
        }
    }

    private void OnSkillChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_skillController.SetSkill(PlayerSkill.Bow);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_skillController.SetSkill(PlayerSkill.FireBall);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_skillController.SetSkill(PlayerSkill.Bomb);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_skillController.SetSkill(PlayerSkill.Hook);
        }
    }

    private void OnSkill(PlayerSkill skillType)
    {
        switch (skillType)
        {
            case PlayerSkill.Bow:
                m_skillController.Fire(m_arrowPositionObject);
                break;
            case PlayerSkill.FireBall:
                m_skillController.Fire(m_firePositionObject);
                break;
            case PlayerSkill.Hook:
                m_skillController.Fire(m_hookPositionObject);
                break;
            case PlayerSkill.Bomb:
                m_skillController.Fire(m_bombPositionObject);
                break;
        }
    }

    private void LookAtMouse()
    {
        Vector3 lookPosition = new Vector3(_plane.Point.x, transform.position.y
            , _plane.Point.z);

        float distance = Vector3.Distance(transform.position, _plane.Point);

        if(distance > 0.1f)
        {
            transform.LookAt(lookPosition);
        }
    }

   

    //AnimationEvent
    public void UseSkillAttack()
    {
        switch (m_skillController.SkillType)
        {
            case PlayerSkill.Bow:
                IsSkill(PlayerSkill.Bow, m_arrowPositionObject);
                break;
            case PlayerSkill.FireBall:
                IsSkill(PlayerSkill.FireBall, m_firePositionObject);
                break;
            case PlayerSkill.Hook:
                IsSkill(PlayerSkill.Hook, m_hookPositionObject);
                OnSkill(PlayerSkill.Hook);
                break;
            case PlayerSkill.Bomb:
                IsSkill(PlayerSkill.Bomb, m_bombPositionObject);
                break;
        }

    }

    private void IsSkill(PlayerSkill skillType, GameObject positionObject)
    {
        bool isSkillUsable = SkillManager.Instance.Cost(skillType);

        if(isSkillUsable)
        {
            m_skillController.UseSkill(positionObject);
        }
    }

    //AnimationEvent
    public void OnCharge()
    {
        m_attackAnimation.SetBool("ChargeAttack", true);
    }

    public void Hit()
    {
        m_weaponController.OnHit(chargeMax);
    }

}
