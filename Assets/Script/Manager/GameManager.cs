using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    #region Player
    private PlayerInput _playerInput;
    public GameObject Player { get; set; }
    #endregion

    #region GameOver
    private bool isGameOver = false;
    public bool IsGameOver { get {  return isGameOver; } set { isGameOver = value; } }
    #endregion

    private void Start()
    {
        if(Player != null)
        {
            _playerInput = Player.GetComponent<PlayerInput>();
        }
    }

    public void PlayerLock(bool isLock)
    {
        _playerInput.enabled = isLock ? false : true;
    }

    public void GameOver()
    {

    }
}
