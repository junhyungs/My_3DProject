using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("SkillObject")]
    [SerializeField] private GameObject[] SkillObject;

    private ISkill m_currentSkill;
    private PlayerSkill m_skillType;
    private Animator m_skillAnimation;

    public ISkill CurrentSkill => m_currentSkill;
    public PlayerSkill SkillType => m_skillType;    

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_skillAnimation = GetComponent<Animator>();
        OnDisableSkillObject();
        m_skillType = PlayerSkill.Bow;
        SkillManager.Instance.AddSkill(m_skillType);
        SkillManager.Instance.AddSkill(PlayerSkill.FireBall);
        SkillManager.Instance.AddSkill(PlayerSkill.Hook);
        SkillManager.Instance.AddSkill(PlayerSkill.Bomb);
        SetSkill(m_skillType);
    }

    public void SetSkill(PlayerSkill skillType)
    {
        if(SkillManager.Instance.HasSkill(skillType))
        {
            Debug.Log("스킬이 없음. 변경불가");
            return;
        }

        m_skillType = skillType;

        Component component = gameObject.GetComponent<ISkill>() as Component;

        if (component != null)
        {
            Destroy(component);
        }

        switch (skillType)
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

        UIManager.Instance.RequestChangeSkill(m_skillType);
        SkillManager.Instance.SetCurretSkill(m_skillType);
    }

    public void CurrentSkillAnimation(bool isPressed)
    {
        switch (m_skillType)
        {
            case PlayerSkill.Bow:
                m_skillAnimation.SetBool("Arrow", isPressed);
                break;
            case PlayerSkill.FireBall:
                m_skillAnimation.SetBool("ArrowMagic", isPressed);
                break;
            case PlayerSkill.Bomb:
                m_skillAnimation.SetBool("Bomb", isPressed);
                break;
            case PlayerSkill.Hook:
                m_skillAnimation.SetTrigger("Hook");
                break;
        }
    }

    public void ActiveSkillObject(bool isPressed)
    {
        SkillObject[(int)m_skillType].SetActive(isPressed);
    }

    public void Fire(GameObject FirePosition)
    {
        m_currentSkill.Fire(FirePosition, true);
    }

    public void UseSkill(GameObject FirePosition)
    {
        m_currentSkill.UseSkill(FirePosition);
    }

    private void OnDisableSkillObject()
    {
        foreach(var skillObj in SkillObject)
        {
            skillObj.SetActive(false);
        }
    }
}
