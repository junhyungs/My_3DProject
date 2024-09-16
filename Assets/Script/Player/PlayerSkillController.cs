using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("SkillObject")]
    [SerializeField] private GameObject[] SkillObject;

    private ISkill m_currentSkill;
    private PlayerSkill _currentSkillType;
    private Animator m_skillAnimation;

    public PlayerSkill SkillType => _currentSkillType;

    private void Awake()
    {
        OnDisableSkillObject();
    }

    private void Start()
    {
        InitializeSkillController();
    }

    private void InitializeSkillController()
    {
        m_skillAnimation = gameObject.GetComponent<Animator>();

        SkillManager.Instance.AddSkill(PlayerSkill.Bow);
        SkillManager.Instance.AddSkill(PlayerSkill.FireBall);
        SkillManager.Instance.AddSkill(PlayerSkill.Bomb);
        SkillManager.Instance.AddSkill(PlayerSkill.Hook);

        _currentSkillType = PlayerSkill.Bow;
        SetSkill(_currentSkillType);
    }

    public void SetSkill(PlayerSkill skillType)
    {
        if(SkillManager.Instance.HasSkill(skillType))
        {
            Debug.Log("스킬이 없음. 변경불가");
            return;
        }

        _currentSkillType = skillType;

        m_currentSkill = null;

        Skill newSkill = SkillManager.Instance.GetSkill(skillType);

        m_currentSkill = newSkill;

        UIManager.Instance.RequestChangeSkill(skillType);

        SkillManager.Instance.SetCurretSkill(skillType);
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

    private void OnDisableSkillObject()
    {
        foreach(var skillObj in SkillObject)
        {
            skillObj.SetActive(false);
        }
    }
}
