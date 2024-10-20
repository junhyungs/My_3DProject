using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneDoor : MonoBehaviour, IInteractionItem
{
    private enum Door
    {
        Open, 
        Close
    }

    [Header("PlayableAsset")]
    [SerializeField] private PlayableAsset[] _asset;

    [Header("MoveMentTransform")]
    [SerializeField] private Transform _movementTransform;

    [Header("ChangeMap")]
    [SerializeField] private Map _changeMap;

    [Header("DifferentTimeLine")]
    [SerializeField] private bool _isDifferentTimeline;

    private HashSet<string> _playerTrack;
    private PlayableDirector _director;
    private GameObject _player;

    private Vector3 _uiPosition = new Vector3(1.8f, 2f, 0f);
    
    private void Awake()
    {
        _director = gameObject.GetComponent<PlayableDirector>();

        _playerTrack = new HashSet<string>()
        {
            "PlayerMovement",
            "PlayerWalkAnimation",
            "PlayerObject"
        };
    }

    private void OnEnable()
    {
        var closeAsset = _asset[(int)Door.Close] as TimelineAsset;

        _director.playableAsset = closeAsset;

        if (_isDifferentTimeline)
        {
            return;
        }

        _director.Play();
    }

    public void InteractionItem()
    {
        var openAsset = _asset[(int)Door.Open] as TimelineAsset;

        _director.playableAsset = openAsset;

        OpenDoor();
    }

    private void OpenDoor()
    {
        GameManager.Instance.PlayerLock(true);

        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.OpenUI);

        ResetPlayerTransform(true);

        BindTimeLine();
    }

    public void CloseDoor()
    {
        if(_player == null)
        {
            _player = GameManager.Instance.Player;
        }

        ResetPlayerTransform(false);

        BindTimeLine();
    }

    private void ResetPlayerTransform(bool isOpen)
    {
        _player.transform.position = _movementTransform.position;

        _player.transform.rotation = isOpen ? Quaternion.Euler(0f, _player.transform.rotation.y, 0f)
            : Quaternion.Euler(0f, -180f, 0f);
    }


    //런타임 중 플레이어 바인딩
    private void BindTimeLine()
    {
        var timeLine = _director;

        var track = timeLine.playableAsset.outputs;

        foreach (var output in track)
        {
            if (_playerTrack.Contains(output.streamName))
            {
                timeLine.SetGenericBinding(output.sourceObject, _player);
            }
        }

        timeLine.Play();
    }

    //문 이동 완료시 플레이어 해제
    public void ReleasePlayer()
    {
        GameManager.Instance.PlayerLock(false);
    }

    //맵 변경
    public void ChangeMap()
    {
        MapManager.Instance.ChangeMap(_changeMap);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(_player == null)
            {
                _player = other.gameObject;
            }

            UIManager.Instance.ItemInteractionUI(transform, _uiPosition, ObjectName.OpenUI);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.OpenUI);
        }
    }
}
