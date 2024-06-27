using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine;


public class Player : MonoBehaviour
{
    private PlayerMoveController m_moveController;
    private PlayerHealth m_playerHealth;
    
    private void Awake()
    {
        GameManager.Instance.Player = this.gameObject;

        m_moveController = GetComponent<PlayerMoveController>();
        m_playerHealth = GetComponent<PlayerHealth>();
    }

}
