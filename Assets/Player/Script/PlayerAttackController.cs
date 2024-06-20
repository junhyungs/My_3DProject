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
    private PlayerMoveController m_moveController;
  
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
        m_moveController = GetComponent<PlayerMoveController>();
        m_weaponController = GetComponent<PlayerWeaponController>();   
        m_skillController = GetComponent<PlayerSkillController>();  
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
            m_moveController.IsAction = false;
            m_weaponController.UseWeapon();
            LookAtMouse();
        }
        else
            m_moveController.IsAction = true;
    }

    private void OnRightClickAttack(InputValue input)
    {
        bool isPress = input.isPressed;

        RightClick(isPress);
    }

    private void RightClick(bool press)
    {
        PlayerSkill skillType = m_skillController.SkillType;

        m_weaponController.ActiveSkillWeaponObject(skillType, press);

        m_skillController.CurrentSkillAnimation(press);

        if (!press)
        {
            OnSkill(skillType);
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

}
