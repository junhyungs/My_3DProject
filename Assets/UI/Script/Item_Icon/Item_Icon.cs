using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Item_Icon : MonoBehaviour
{
    [Header("SoulText")]
    [SerializeField] private Text SoulValueText;

    [Header("HealthText")]
    [SerializeField] private Text HealthValueText;

    ItemViewModel m_ItemView;

    private void OnEnable()
    {
        if(m_ItemView == null)
        {
            m_ItemView = new ItemViewModel();

            m_ItemView.PropertyChanged += OnItem_TextValueChange;

            m_ItemView.RegisterChangeValueEventOnEnable();
        }
    }

    private void OnItem_TextValueChange(object sender, PropertyChangedEventArgs arg)
    {
        switch(arg.PropertyName)
        {
            case nameof(m_ItemView.SoulCount):
                SoulValueText.text = "X " + m_ItemView.SoulCount.ToString();
                break;
            case nameof(m_ItemView.HealthCount):
                HealthValueText.text = "X " + m_ItemView.HealthCount.ToString();
                break;
        }
    }
}
