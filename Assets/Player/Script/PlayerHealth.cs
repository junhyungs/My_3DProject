using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamged
{
    private int m_playerHp;
    public int PlayerHP { get { return m_playerHp; } set { m_playerHp = value; } }

    public void TakeDamage(int damage)
    {
        m_playerHp -= damage;


        if(m_playerHp <= 0)
        {

        }
    }
}
