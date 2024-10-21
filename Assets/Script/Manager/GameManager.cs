using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    #region Player
    private PlayerInput _playerInput;
    private Animator _animator;
    private PlayerHealth _health;
    private PlayerMoveController _moveController;
    private Func<IEnumerator> _deathCameraZoom;
    public GameObject Player { get; set; }
    #endregion

    #region GameOver
    private bool isGameOver = false;
    public bool IsGameOver { get {  return isGameOver; } set { isGameOver = value; } }
    #endregion

    #region CreatePlayer
    private const string _playerPath = "Prefab/Player/player";
    public bool IsCreate { get; set; }
    #endregion

    private void Awake()
    {
        CreatePlayer();
    }

    private void Start()
    {
        if(Player != null)
        {
            _playerInput = Player.GetComponent<PlayerInput>();
        }
    }

    public void CreatePlayer()
    {
        GameObject player = Resources.Load<GameObject>(_playerPath);

        player.SetActive(false);

        Player = Instantiate(player);

        _playerInput = Player.GetComponent<PlayerInput>();

        _health = Player.GetComponent<PlayerHealth>();

        _animator = Player.GetComponent<Animator>();

        _moveController = Player.GetComponent<PlayerMoveController>();

        IsCreate = true;
    }

    public void PlayerLock(bool isLock)
    {
        _playerInput.enabled = isLock ? false : true;
    }
    public void RegisterDeathAction(Func<IEnumerator> callBack)
    {
        _deathCameraZoom = callBack;
    }

    public void GameOver()
    {
        StartCoroutine(PlayerDeathCoroutine());
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        PlayerLock(true);

        yield return StartCoroutine(_deathCameraZoom.Invoke());

        UIManager.Instance.OnDeathUI(true);

        yield return new WaitForSeconds(4f);

        UIManager.Instance.OnDeathUI(false);

        yield return new WaitForSeconds(1f);

        Player.SetActive(false);

        MapManager.Instance.Respawn();

        Respawn();
    }

    private void Respawn()
    {
        Player.gameObject.layer = LayerMask.NameToLayer("Player");

        _health.PlayerHP = 4;

        _animator.SetBool("Die", false);

        _moveController.IsAction = true;
    }

}
