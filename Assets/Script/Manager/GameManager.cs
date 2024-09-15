using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isGameOver = false;

    public GameObject Player { get; set; }
    public bool IsGameOver { get {  return isGameOver; } set { isGameOver = value; } }

    [Header("RespawnPosition")]
    [SerializeField] private Transform m_respawnTransform;
    public Transform RespawnTransform { get { return m_respawnTransform;} }

    
    public void RespawnPlayer()
    {
        if (isGameOver)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.0f);

        PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();

        playerHealth.PlayerHP = 4;

        Animator playerAnim = Player.GetComponent<Animator>();

        playerAnim.SetBool("Die", false);

        PlayerMoveController playerMoveController = Player.GetComponent<PlayerMoveController>();

        playerMoveController.IsAction = true;

        Player.transform.position = m_respawnTransform.position;

        Player.SetActive(true);

        isGameOver = false;
    }

}
