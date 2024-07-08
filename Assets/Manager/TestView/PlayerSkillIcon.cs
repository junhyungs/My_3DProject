
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
    

    private void Start()
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

    private Image GetSkill_Icon(PlayerSkill skill)
    {
        return Skill_IconDic[skill];
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
                Debug.Log(m_currentSkill);
                IconColorChanged(m_SkillView.CurrentSkill);
                break;
        }
    }

    private void IconColorChanged(PlayerSkill currentSkill)
    {
        Image preimg = GetSkill_Icon(m_currentSkill);
        Color color = Color.gray;
        preimg.color = color;   

        switch(currentSkill)
        {
            case PlayerSkill.Bow:
                Image bowColor = GetSkill_Icon(PlayerSkill.Bow);
                Color changeColor = Color.white;
                bowColor.color = changeColor;
                break;
            case PlayerSkill.FireBall:
                Image fireImg = GetSkill_Icon(PlayerSkill.FireBall);
                Color fireColor = Color.white;
                fireImg.color = fireColor;
                break;
            case PlayerSkill.Bomb:
                Image bombImg = GetSkill_Icon(PlayerSkill.Bomb);
                Color bombColor = Color.white;
                bombImg.color = bombColor;
                break;
            case PlayerSkill.Hook:
                Image hookImg = GetSkill_Icon(PlayerSkill.Hook);
                Color hookColor = Color.white;
                hookImg.color = hookColor;
                break;
        }
    }
}
