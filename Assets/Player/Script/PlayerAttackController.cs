using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [Header("ProjectilePositionObject")]
    [SerializeField] private GameObject m_arrowPositionObject;

    private PlayerWeaponController m_weaponController;
    private PlayerSkillController m_skillController;
    private Animator m_attackAnimation;
    private bool chargeMax;
    private bool chargeAttackDirection = true;
    private int m_attackCount;

    public bool IsCharge
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
        if (chargeAttackDirection)
        {
            m_attackAnimation.SetTrigger("ChargeAttackR");
        }
        else
        {
            m_attackAnimation.SetTrigger("ChargeAttackL");
        }


        if (!isPressed && chargeMax)
        {
            m_attackAnimation.SetBool("ChargeAttack", false);
            m_weaponController.UseWeapon(true);
            chargeAttackDirection = !chargeAttackDirection;
        }
        else if(!isPressed && !chargeMax)
        {
            m_attackAnimation.SetTrigger("ChargeFail");
        }
    }

    private void OnSkill(PlayerSkill skillType)
    {
        switch (skillType)
        {
            case PlayerSkill.Bow:
                m_skillController.Fire(m_arrowPositionObject);
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
        if (SkillManager.Instance.SkillCount == 0)
        {
            return;
        }

        switch (m_skillController.SkillType)
        {
            case PlayerSkill.Bow:
                m_skillController.UseSkill(m_arrowPositionObject);
                SkillManager.Instance.SkillCount--;
                break;
        }

    }

    public void OnCharge()
    {
        chargeMax = true;
    }

}
