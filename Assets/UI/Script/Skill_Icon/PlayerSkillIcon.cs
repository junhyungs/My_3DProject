using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillIcon : MonoBehaviour
{
    [Header("IconBow")]
    [SerializeField] private Image BowIcon;

    [Header("IconFireBall")]
    [SerializeField] private Image FireBallIcon;

    [Header("IconBomb")]
    [SerializeField] private Image BombIcon;

    [Header("IconHook")]
    [SerializeField] private Image HookIcon;

    [Header("SkillCountIcon")]
    [SerializeField] private List<GameObject> EnableSkillCount_IconList = new List<GameObject>();

    [Header("CurrentSkill")]
    [SerializeField] private MVVM _currentSkill;
    [Header("SkillCount")]
    [SerializeField] private MVVM _skillCount;

    PlayerSkillViewModel m_SkillView;
    private Dictionary<PlayerSkill, Image> Skill_IconDic = new Dictionary<PlayerSkill, Image>();

    private void OnEnable()
    {
        DisableCount_Icon();

        if (m_SkillView == null)
        {
            m_SkillView = new PlayerSkillViewModel();

            m_SkillView.PropertyChanged += OnSkill_IconColorChanged;

            m_SkillView.RegisterChangeSkillIcon_EventOnEnable(_currentSkill, _skillCount);

            initIconDic();
        }

    }

    private void initIconDic()
    {
        Skill_IconDic.Add(PlayerSkill.Bow, BowIcon);
        Skill_IconDic.Add(PlayerSkill.FireBall, FireBallIcon);
        Skill_IconDic.Add(PlayerSkill.Bomb, BombIcon);
        Skill_IconDic.Add(PlayerSkill.Hook, HookIcon);
    }

    private void OnSkill_IconColorChanged(object sender, PropertyChangedEventArgs arg)
    {
        switch(arg.PropertyName)
        {
            case nameof(m_SkillView.CurrentSkill):
                IconColorChanged(m_SkillView.CurrentSkill);
                break;
            case nameof(m_SkillView.CurrentSkillCount):
                ChangeSkillCountIcon(m_SkillView.CurrentSkillCount);
                break;
        }
    }

    private void IconColorChanged(PlayerSkill currentSkill)
    {
        foreach (var icon in Skill_IconDic.Values)
        {
            icon.color = Color.gray;
        }

        if (Skill_IconDic.TryGetValue(currentSkill, out Image currentIcon))
        {
            currentIcon.color = Color.white;
        }
    }

    private void ChangeSkillCountIcon(int skillCount)
    {
        DisableCount_Icon();

        for(int i = 0; i < skillCount; i++)
        {
            EnableSkillCount_IconList[i].SetActive(true);
        }
    }

    private void DisableCount_Icon()
    {
        foreach(var icon in EnableSkillCount_IconList)
        {
            icon.SetActive(false);
        }
    }

}
