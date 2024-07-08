using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerHpViewModel
{
    private int m_currentHp;

    public int CurrentHp
    {
        get { return m_currentHp; }
        set
        {
            if(m_currentHp == value)
            {
                return;
            }

            m_currentHp = value;
            OnPropertyChanged(nameof(CurrentHp));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
