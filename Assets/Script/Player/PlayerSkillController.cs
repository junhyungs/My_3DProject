using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("SkillObject")]
    [SerializeField] private GameObject[] SkillObject;

    private Animator m_skillAnimation;

    private ISkill m_currentSkill;
    private PlayerSkill _currentSkillType;

    public PlayerSkill SkillType => _currentSkillType;

    private void Awake()
    {
        m_skillAnimation = gameObject.GetComponent<Animator>();

        OnAwakeSkillObject();
    }

    private void OnAwakeSkillObject()
    {
        foreach (var skillObject in SkillObject)
        {
            if (skillObject != null)
            {
                skillObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        SetSkill(PlayerSkill.Bow);
    }

    public void SetSkill(PlayerSkill skillType)
    {
        _currentSkillType = skillType;

        if(m_currentSkill != null)
        {
            m_currentSkill = null;
        }

        var newSkill = SkillManager.Instance.GetSkill(skillType);

        if(newSkill == null)
        {
            StartCoroutine(GetSkill(skillType));
        }
        else
        {
            m_currentSkill = newSkill;
        }

        UIManager.Instance.TriggerEvent(MVVM.CurrentSkill_Event, _currentSkillType);
    }

    private IEnumerator GetSkill(PlayerSkill skillType)
    {
        yield return new WaitUntil(() =>
        {
            return SkillManager.Instance.GetSkill(skillType) != null;
        });

        var skill = SkillManager.Instance.GetSkill(skillType);

        m_currentSkill = skill;
    }

    public void CurrentSkillAnimation(bool isPressed)
    {
        switch (_currentSkillType)
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
        SkillObject[(int)_currentSkillType].SetActive(isPressed);
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
