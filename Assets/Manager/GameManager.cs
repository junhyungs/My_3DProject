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

    


}
