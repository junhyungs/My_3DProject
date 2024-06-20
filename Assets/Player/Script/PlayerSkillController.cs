using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
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
        m_skillType = PlayerSkill.Bow;
        SetSkill(m_skillType);
    }

    public void SetSkill(PlayerSkill skillType)
    {
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

    }

    public void CurrentSkillAnimation(bool isPressed)
    {
        switch (m_skillType)
        {
            case PlayerSkill.Bow:
                m_skillAnimation.SetBool("Arrow", isPressed);
                break;
            case PlayerSkill.FireBall:
                //ÆÄÀÌ¾îº¼
                break;
            case PlayerSkill.Bomb:
                //ÆøÅº
                break;
            case PlayerSkill.Hook:
                //°¥°í¸®
                break;
        }
    }

    public void Fire(GameObject FirePosition)
    {
        m_currentSkill.Fire(FirePosition, true);
    }

    public void UseSkill(GameObject FirePosition)
    {
        m_currentSkill.UseSkill(FirePosition);
    }
}
