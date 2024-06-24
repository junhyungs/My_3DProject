using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [Header("ProjectilePositionObject")]
    [SerializeField] private GameObject m_arrowPositionObject;
    [SerializeField] private GameObject m_firePositionObject;
    

    private PlayerWeaponController m_weaponController;
    private PlayerSkillController m_skillController;
    private Animator m_attackAnimation;
   
    private bool chargeMax;
    private bool chargeAttackDirection = true;


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
        if(Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            LookAtMouse();
        }

        OnSkillChange();
    }

    private void Init()
    {
        m_weaponController = GetComponent<PlayerWeaponController>();   
        m_skillController = GetComponent<PlayerSkillController>();  
        m_attackAnimation = GetComponent<Animator>();
    }

    private void OnLeftClickAttack(InputValue input)
    {
        bool isPressed = input.isPressed;

        LeftClick(isPressed);
    }

    private void LeftClick(bool isPressed)
    {
        if (isPressed)
        {
            m_weaponController.UseWeapon(false);
            chargeAttackDirection = !chargeAttackDirection;
            LookAtMouse();
        }
    }

    private void OnRightClickAttack(InputValue input)
    {
        bool isPressed = input.isPressed;

        RightClick(isPressed);
    }

    private void RightClick(bool isPressed)
    {
        PlayerSkill skillType = m_skillController.SkillType;

        m_weaponController.ActiveSkillWeaponObject(skillType, isPressed);

        m_skillController.CurrentSkillAnimation(isPressed);

        if (!isPressed)
        {
            OnSkill(skillType);
        }

    }

    private void OnChargeAttack(InputValue input)
    {
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
            m_weaponController.UseWeapon(true);
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
        }
    }

    private void LookAtMouse()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );

        if(Physics.Raycast(mouseRay, out RaycastHit hit, 100))
        {
            Vector3 lookPosition = new Vector3(hit.point.x,transform.position.y, hit.point.z);

            float distance = Vector3.Distance(transform.position, hit.point);

            if(distance > 0.1f)
            {
                transform.LookAt(lookPosition);
            }
        }
    }

    //AnimationEvent
    public void UseSkillAttack()
    {
        if (SkillManager.Instance.SkillCount <= 0)
            return;

        int cost;

        switch (m_skillController.SkillType)
        {
            case PlayerSkill.Bow:
                cost = SkillManager.Instance.GetSkillData(PlayerSkill.Bow).m_cost;
                SkillManager.Instance.SkillCount -= cost;
                m_skillController.UseSkill(m_arrowPositionObject);
                break;
            case PlayerSkill.FireBall:
                cost = SkillManager.Instance.GetSkillData(PlayerSkill.FireBall).m_cost;
                SkillManager.Instance.SkillCount -= cost;   
                m_skillController.UseSkill(m_firePositionObject);
                break;
        }

    }

    //AnimationEvent
    public void OnCharge()
    {
        m_attackAnimation.SetBool("ChargeAttack", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
