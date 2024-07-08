using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerSkillViewModel
{
    private string skillName;
    private PlayerSkill currentSkill;
    private Color IconColor;

    public string SkillName
    {
        get { return skillName; }
        set
        {
            if(skillName == value)
            {
                return;
            }

            skillName = value;
            OnPropertyChanged(nameof(skillName));
        }
    }

    public PlayerSkill CurrentSkill
    {
        get { return currentSkill; }
        set
        {
            if(currentSkill == value)
            {
                return;
            }

            currentSkill = value;
            OnPropertyChanged(nameof(CurrentSkill));
        }
    }

    public Color CurrentColor
    {
        get { return IconColor; }
        set
        {
            if(IconColor == value)
            {
                return;
            }

            IconColor = value;
            OnPropertyChanged(nameof(IconColor));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }   




}
