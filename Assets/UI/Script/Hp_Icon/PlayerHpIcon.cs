using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpIcon : MonoBehaviour
{
    [Header("PlayerHPIcon")]
    [SerializeField] private List<Image> HpIconList = new List<Image>();

    [Header("Key")]
    [SerializeField] private MVVM _keyName;
    

    PlayerHpViewModel m_hpView;

    private int m_currentHp;

    private void OnEnable()
    {
        if(m_hpView == null)
        {
            m_hpView = new PlayerHpViewModel();

            m_hpView.PropertyChanged += PlayerHpIconChanged;

            m_hpView.RegisterChangeHpEventOnEnable(_keyName);
        }
    }

    private void PlayerHpIconChanged(object sender, PropertyChangedEventArgs arg)
    {
        if(arg.PropertyName == nameof(m_hpView.CurrentHp))
        {
            m_currentHp = m_hpView.CurrentHp;
            HpIconChange(m_currentHp);
        }
    }

    private void HpIconChange(int hp)
    {
        foreach(var icon in HpIconList)
        {
            icon.color = Color.gray;
        }

        for(int i = 0; i < hp && i < HpIconList.Count; i++)
        {
            HpIconList[i].color = Color.green;
        }

    }

}
