using Cinemachine.Utility;
using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerAttackController : MonoBehaviour, IHitEvent
{
    [Header("ProjectilePositionObject")]
    [SerializeField] private GameObject m_arrowPositionObject;
    [SerializeField] private GameObject m_firePositionObject;
    [SerializeField] private GameObject m_hookPositionObject;
    [SerializeField] private GameObject m_bombPositionObject;

    private GameObject[] m_PositionObject;
    private PlayerWeaponController m_weaponController;
    private PlayerSkillController m_skillController;
    private CharacterController m_playerController;
    private Animator m_attackAnimation;

    private Action<bool> m_hitEvent;
   
    private bool chargeMax;
    private bool chargeAttackDirection = true;
    private bool isAction = true;
    private bool isFlying = false;

    private bool _iscollide = false;

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

    private void Start()
    {
        EventManager.Instance.AddEvent_HookPositionEvent(true, OnHookCollied);
    }

    private void OnDestroy()
    {
        EventManager.Instance.AddEvent_HookPositionEvent(false, OnHookCollied);
    }

    private void Update()
    {
        OnUpdate();
    }

    private void Init()
    {
        m_weaponController = GetComponent<PlayerWeaponController>();   
        m_skillController = GetComponent<PlayerSkillController>();  
        m_playerController = GetComponent<CharacterController>();
        m_attackAnimation = GetComponent<Animator>();
        EventManager.Instance.RegisterOverlapBoxEvent(this);

        m_PositionObject = new GameObject[4];
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

    // HookMove
    public void OnHookCollied(Vector3 targetPos, bool isAnchor)
    {
        
        if (isAnchor)
        {
            m_attackAnimation.SetTrigger("HookStart");
            m_attackAnimation.SetBool("HookEnd", true);
            gameObject.layer = LayerMask.NameToLayer("fly");
            StartCoroutine(Hook(targetPos));
        }
        else
            m_attackAnimation.SetTrigger("HookFail");
    }

    private IEnumerator Hook(Vector3 targetPosition)
    {
        float hookSpeed = 15f;
        float maxDistance = 10f;
        float currentDistance = 0f;

        Vector3 hookDirection = (targetPosition - transform.position).normalized;

        LayerMask targetLayer = LayerMask.GetMask("HookObject", "Monster");

        hookDirection.y = 0f;

        _iscollide = false;

        while (true)
        {
            _iscollide = Physics.CheckSphere(transform.position, 1f, targetLayer);

            if (_iscollide)
            {
                m_attackAnimation.SetBool("HookEnd", false);
                gameObject.layer = LayerMask.NameToLayer("Player");

                Collider[] removeChain = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Hook"));

                if(removeChain.Length > 0)
                {
                    foreach (var chain in removeChain)
                    {
                        PoolManager.Instance.ReturnPlayerSegment(chain.gameObject);
                    }
                }

                break;
            }
            else
            {
                float distance = hookSpeed * Time.deltaTime;

                currentDistance += distance;

                if(currentDistance >= maxDistance)
                {
                    m_attackAnimation.SetBool("HookEnd", false);
                    gameObject.layer = LayerMask.NameToLayer("Player");
                    break;
                }

                Vector3 moveVector = hookDirection * hookSpeed * Time.deltaTime;

                m_playerController.Move(moveVector);

                yield return null;
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
        if (SkillManager.Instance.SkillCount <= 0 && m_skillController.SkillType != PlayerSkill.Hook)
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
            case PlayerSkill.Hook:
                m_skillController.UseSkill(m_hookPositionObject);
                OnSkill(PlayerSkill.Hook);
                break;
            case PlayerSkill.Bomb:
                cost = SkillManager.Instance.GetSkillData(PlayerSkill.Bomb).m_cost;
                SkillManager.Instance.SkillCount -= cost;
                m_skillController.UseSkill(m_bombPositionObject);
                break;
        }

    }

    //AnimationEvent
    public void OnCharge()
    {
        m_attackAnimation.SetBool("ChargeAttack", true);
    }

    public void Hit()
    {
        m_hitEvent?.Invoke(chargeMax);
    }

    public void HitOverlapBox(bool isAddEvent, Action<bool> callBack)
    {
        if (isAddEvent)
        {
            m_hitEvent += callBack;
        }
        else
        {
            m_hitEvent -= callBack;
        }
    }
}