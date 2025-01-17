using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour, IInteractionItem
{
    [Header("ExitUI")]
    [SerializeField] private GameObject _exitUI;

    [Header("Intransform")]
    [SerializeField] private Transform _inTransform;

    private PlayableDirector _director;
    private GameObject _player;
    private HashSet<string> _track;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();

        _track = new HashSet<string>
        {
            "PlayerMovement",
            "PlayerWalkAnimation",
            "PlayerObject"
        };
    }

    private void OnDisable()
    {
        UIManager.Instance.OnLoadingUI(false);
    }

    private void Start()
    {
        _exitUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(_player == null)
            {
                _player = other.gameObject;
            }

            _exitUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _exitUI.SetActive(false);
        }
    }

    public void InteractionItem()
    {
        _exitUI.SetActive(false);

        if (_player == null)
        {
            _player = GameManager.Instance.Player;
        }

        ResetPlayerTransform(_player);

        var track = _director.playableAsset.outputs;

        foreach(var output in track)
        {
            if (_track.Contains(output.streamName))
            {
                _director.SetGenericBinding(output.sourceObject, _player);
            }
        }

        _director.Play();
    }

    //¾À º¯°æ
    public void StartScene()
    {
        StartCoroutine(Exit());
    }

    private IEnumerator Exit()
    {
        yield return null;

        UIManager.Instance.OnLoadingUI(true);

        SceneManager.LoadScene("StartScene");
    }

    private void ResetPlayerTransform(GameObject player)
    {
        player.transform.position = _inTransform.position;

        Vector3 rotateDirection = transform.position - player.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);

        player.transform.rotation = targetRotation;

    }
}
