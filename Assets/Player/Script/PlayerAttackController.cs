using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [Header("PlayerSkill_Object")]
    [SerializeField] private GameObject[] m_skillObject;

    [Header("ProjectilePosition")]
    [SerializeField] private Transform m_arrowTransform;

    private Animator m_playerAnimator;
    private ISkill m_currentSkill;
    private IWeapon m_currentWeapon;
    private PlayerWeapon m_weaponType;
    private PlayerSkill m_SkillName;

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
        m_playerAnimator = GetComponent<Animator>();
        m_SkillName = PlayerSkill.Bow;
        SetSkill(m_SkillName);
        SetWeapon();
        DisableObject();
    }

    public void SetSkill(PlayerSkill skillName)
    {
        m_SkillName = skillName;

        Component component = gameObject.GetComponent<ISkill>() as Component;

        if (component != null)
        {
            Destroy(component);
        }

        switch (m_SkillName)
        {
            case PlayerSkill.Bow:
                m_currentSkill = gameObject.AddComponent<Bow>();
                break;
            case PlayerSkill.FireBall:
                m_currentSkill = gameObject.AddComponent<FireBall>();
                break;
            case PlayerSkill.Bomb:
                m_currentSkill = gameObject.AddComponent<Bomb>();
                break;
            case PlayerSkill.Hook:
                m_currentSkill = gameObject.AddComponent<Hook>();
                break;
        }

    }

    public void SetWeapon()
    {
        //지금은 웨폰 매니저의 참조를 받지만 나중에는 UI -> 참조 클래스 -> 를 거쳐서 SetWeapon을 호출하도록 변경.
        m_weaponType = WeaponManager.Instance.GetCurrentWeapon();

        Component component = gameObject.GetComponent<IWeapon>() as Component;

        if(component != null)
        {
            Destroy(component);
        }

        switch(m_weaponType)
        {
            case PlayerWeapon.Sword:
                m_currentWeapon = gameObject.AddComponent<Sword>();
                break;
            case PlayerWeapon.Hammer:
                m_currentWeapon = gameObject.AddComponent<Hammer>();
                break;
            case PlayerWeapon.Dagger:
                m_currentWeapon = gameObject.AddComponent<Dagger>();
                break;
            case PlayerWeapon.GreatSword:
                m_currentWeapon = gameObject.AddComponent<GreatSword>();
                break;
            case PlayerWeapon.Umbrella:
                m_currentWeapon = gameObject.AddComponent<Umbrella>();
                break;
        }


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
            UseAttack();

            LookAtMouse();
        }
    }

    private void OnRightClickAttack(InputValue input)
    {
        bool isPress = input.isPressed;

        RightClick(isPress);
    }

    private void RightClick(bool press)
    {
        ActiveSkillObject(m_SkillName, press);

        OnCurrentSkillAnimation(m_SkillName, press);

        if (!press)
        {
            m_currentSkill.Fire(true);
        }
    }

    private void ActiveSkillObject(PlayerSkill skillName, bool isPressed)
    {
        m_skillObject[(int)skillName].SetActive(isPressed);
    }

    private void OnCurrentSkillAnimation(PlayerSkill skillName, bool isPressed)
    {
        switch(skillName)
        {
            case PlayerSkill.Bow:
                m_playerAnimator.SetBool("Arrow", isPressed);
                break;
            case PlayerSkill.FireBall:
                //파이어볼
                break;
            case PlayerSkill.Bomb:
                //폭탄
                break;
            case PlayerSkill.Hook:
                //갈고리
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

    private void DisableObject()
    {
        foreach(var skillObj in m_skillObject)
        {
            if(skillObj != null)
            {
                skillObj.SetActive(false);
            }
        }
    }

    private void UseAttack()
    {
        m_currentWeapon.UseWeapon();
    }

    //AnimationEvent
    public void UseSkillAttack()
    {
        switch (m_SkillName)
        {
            case PlayerSkill.Bow:
                m_currentSkill.UseSkill(m_arrowTransform);
                break;
        }

    }

}
