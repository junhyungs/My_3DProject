using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ItemViewModel
{
    private int m_soulCount;
    private int m_HealthCount;

    public int SoulCount
    {
        get { return m_soulCount; }
        set
        {
            if(m_soulCount == value)
            {
                return;
            }

            m_soulCount = value;
            OnPropertyChanged(nameof(SoulCount));
        }
    }

    public int HealthCount
    {
        get { return m_HealthCount; }
        set
        {
            if(m_HealthCount == value)
            {
                return;
            }

            m_HealthCount = value;
            OnPropertyChanged(nameof(HealthCount));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
