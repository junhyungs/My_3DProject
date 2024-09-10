using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameObject player;
    private bool isGameOver = false;

    public GameObject Player { get { return player; } set { player = value; } }
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

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        playerHealth.PlayerHP = 4;

        Animator playerAnim = player.GetComponent<Animator>();

        playerAnim.SetBool("Die", false);

        PlayerMoveController playerMoveController = player.GetComponent<PlayerMoveController>();

        playerMoveController.IsAction = true;

        player.transform.position = m_respawnTransform.position;

        player.SetActive(true);

        isGameOver = false;
    }

}
