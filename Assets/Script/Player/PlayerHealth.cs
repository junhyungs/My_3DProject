using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamged
{
    private Player _player;
    private Animator m_hitAnimator;

    private int m_playerHp;

    private void Awake()
    {
        m_hitAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _player = GetComponent<Player>();   
    }

    public void SetHealthData(int hp)
    {
        m_playerHp = hp;
        UIManager.Instance.RequestChangeHp(m_playerHp);
    }

    public int PlayerHP
    {
        get { return m_playerHp; }
        set
        {
            m_playerHp = value;
            UIManager.Instance.RequestChangeHp(m_playerHp);
        }
    }

    public void TakeDamage(float damage)
    {
        m_playerHp -= (int)damage;

        UIManager.Instance.RequestChangeHp(m_playerHp);

        m_hitAnimator.SetTrigger("Hit");
        
        if(m_playerHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        m_hitAnimator.SetBool("Die", true);

        GameManager.Instance.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            m_playerHp -= 1;

            UIManager.Instance.RequestChangeHp(m_playerHp);

            if (m_playerHp <= 0)
            {
                _player.DestroyShadowPlayer();

                Die();

                return;
            }

            gameObject.SetActive(false);

            Transform respawn = _player.ShadowPlayer.transform;

            transform.position = respawn.position;

            gameObject.SetActive(true);
        }
    }
}
