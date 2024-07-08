
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

    PlayerSkillViewModel m_SkillView;

    private string m_currentSkillName;

    private PlayerSkill m_currentSkill;

    private Dictionary<PlayerSkill, Image> Skill_IconDic = new Dictionary<PlayerSkill, Image>();
    

    private void OnEnable()
    {
        initIconDic();

        if(m_SkillView == null)
        {
            m_SkillView = new PlayerSkillViewModel();

            m_SkillView.PropertyChanged += OnPropertyChanged;

            m_SkillView.RegisterChangeSkillEventOnEnable();

            m_SkillView.RefreshViewModel();
        }

    }

    private void initIconDic()
    {
        Skill_IconDic.Add(PlayerSkill.Bow, BowIcon);
        Skill_IconDic.Add(PlayerSkill.FireBall, FireBallIcon);
        Skill_IconDic.Add(PlayerSkill.Bomb, BombIcon);
        Skill_IconDic.Add(PlayerSkill.Hook, HookIcon);
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs arg)
    {
        switch(arg.PropertyName)
        {
            case nameof(m_SkillView.SkillName):
                m_currentSkillName = m_SkillView.SkillName;
                break;
            case nameof(m_SkillView.CurrentSkill):
                m_currentSkill = m_SkillView.CurrentSkill;
                IconColorChanged(m_SkillView.CurrentSkill);
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

}
