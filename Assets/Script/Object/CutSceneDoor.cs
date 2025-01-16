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
        Close,
    }

    [Header("PlayableAsset")]
    [SerializeField] private PlayableAsset[] _asset;

    [Header("InTransform")]
    [SerializeField] private Transform _inTransform;

    [Header("OutPosition")]
    [SerializeField] private Transform _outTransform;

    [Header("ChangeMap")]
    [SerializeField] private Map _changeMap;

    private HashSet<string> _playerTrack;
    private PlayableDirector _director;
    private GameObject _player;
    private WaitForSeconds _nameWait = new WaitForSeconds(1f);

    private Vector3 _uiPosition = new Vector3(1.8f, 2f, 0f);
    
    private void Awake()
    {
        if(_director == null)
        {
            Initialize();
        }
    }

    private void OnEnable()
    {
        _director.stopped += DirectorStoppped;
    }

    private void OnDisable()
    {
        _director.stopped -= DirectorStoppped;
    }

    public void InteractionItem()
    {
        OpenDoor();
    }

    private void DirectorStoppped(PlayableDirector director)
    {
        StartCoroutine(OnMapName());
    }

    private IEnumerator OnMapName()
    {
        yield return _nameWait;

        UIManager.Instance.OnMapNameUI();
    }

    private void OpenDoor()
    {
        var openAsset = _asset[(int)Door.Open] as TimelineAsset;

        _director.playableAsset = openAsset;

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

        if(_director == null)
        {
            Initialize();
        }

        var closeAsset = _asset[(int)Door.Close] as TimelineAsset;

        _director.playableAsset = closeAsset;

        ResetPlayerTransform(false);

        BindTimeLine();
    }

    private void Initialize()
    {
        _director = gameObject.GetComponent<PlayableDirector>();

        _playerTrack = new HashSet<string>()
        {
            "PlayerMovement",
            "PlayerWalkAnimation",
            "PlayerObject"
        };
    }

    private void ResetPlayerTransform(bool isOpen)
    {
        _player.transform.position = isOpen ? _inTransform.position : _outTransform.position;

        Vector3 rotateDirection = transform.position - _player.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);

        _player.transform.rotation = targetRotation;

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
