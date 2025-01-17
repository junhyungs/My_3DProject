using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    #region Player
    private PlayerInput _playerInput;
    private PlayerHealth _health;
    private Animator _animator;
    private Func<IEnumerator> _deathCameraZoom;
    public GameObject Player { get; set; }
    #endregion

    #region CreatePlayer
    private const string _playerPath = "Prefab/Player/player";
    #endregion

    private void Awake()
    {
        CreatePlayer();
    }

    private void Start()
    {
        GetPlayerComponent();
    }

    private void CreatePlayer()
    {
        GameObject player = Resources.Load<GameObject>(_playerPath);

        Player = Instantiate(player);
    }

    private void GetPlayerComponent()
    {
        if(Player != null)
        {
            _playerInput = Player.GetComponent<PlayerInput>();

            _health = Player.GetComponent<PlayerHealth>();

            _animator = Player.GetComponent<Animator>();

            Player.SetActive(false);
        }
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

        UIManager.Instance.OnLoadingUI(true);

        var playerComponent = Player.GetComponent<Player>();

        Destroy(playerComponent.ShadowPlayer.gameObject);

        Player.SetActive(false);

        MapManager.Instance.Respawn(playerComponent);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.5f);

        Player.SetActive(true);

        UIManager.Instance.OnLoadingUI(false);

        Player.gameObject.layer = LayerMask.NameToLayer("Player");

        _health.PlayerHP = 4;

        _animator.SetBool("Die", false);

        PlayerLock(false);
    }

}
