
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
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

    PlayerSkillViewModel m_SkillView;

    private PlayerSkill m_currentSkill;
    private int m_currentSkillCount;

    private Dictionary<PlayerSkill, Image> Skill_IconDic = new Dictionary<PlayerSkill, Image>();
    

    private void OnEnable()
    {
        initIconDic();
        DisableCount_Icon();

        if (m_SkillView == null)
        {
            m_SkillView = new PlayerSkillViewModel();

            m_SkillView.PropertyChanged += OnSkill_IconColorChanged;

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

    private void OnSkill_IconColorChanged(object sender, PropertyChangedEventArgs arg)
    {
        switch(arg.PropertyName)
        {
            case nameof(m_SkillView.CurrentSkill):
                m_currentSkill = m_SkillView.CurrentSkill;
                IconColorChanged(m_currentSkill);
                break;
            case nameof(m_SkillView.CurrentSkillCount):
                m_currentSkillCount = m_SkillView.CurrentSkillCount;
                ChangeSkillCountIcon(m_currentSkillCount);
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
