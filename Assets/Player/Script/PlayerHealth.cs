using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamged
{
    private int m_playerHp;
    private Animator m_hitAnimator;

    private void Awake()
    {
        m_playerHp = 4;
        m_hitAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        m_playerHp -= (int)damage;

        m_hitAnimator.SetTrigger("Hit");
        
        if(m_playerHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        m_hitAnimator.SetBool("Die", true);
        GameManager.Instance.IsGameOver = true;
    }
}
