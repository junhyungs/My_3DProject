using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerSkillViewModel
{
    private PlayerSkill currentSkill = PlayerSkill.None;
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

    private int currentSkillCount;

    public int CurrentSkillCount
    {
        get { return currentSkillCount; }
        set
        {
            if(currentSkillCount == value)
            {
                return;
            }

            currentSkillCount = value;
            OnPropertyChanged(nameof(CurrentSkillCount));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }   
}
