using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneDoor : MonoBehaviour, IInteractionItem
{
    [Header("InternalDoor")]
    [SerializeField] private GameObject _internalDoor;

    [Header("MoveMentTransform")]
    [SerializeField] private Transform _movementTransform;

    [Header("UI_Position")]
    [SerializeField] private Vector3 _uiPosition;

    [Header("ChangeMap")]
    [SerializeField] private Map _changeMap;

    private BoxCollider _boxCollider;
    private GameObject _player;

    private HashSet<string> _playerTrack;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();

        _playerTrack = new HashSet<string>()
        {
            "PlayerMovement",
            "PlayerWalkAnimation",
            "PlayerObject"
        };
    }

    private void Start()
    {
        _internalDoor.SetActive(false);
    }

    public void InteractionItem()
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
        PlayerLock(true);

        ResetPlayerTransform();

        ColliderControl(false);

        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.OpenUI);

        BindTimeLine(TimeLineType.Move);
    }

    public void CloseDoor()
    {
        _player = GameManager.Instance.Player;

        ResetPlayerTransform();

        BindTimeLine(TimeLineType.Out);
    }

    private void ResetPlayerTransform()
    {
        _player.transform.position = _movementTransform.position;

        _player.transform.rotation = Quaternion.Euler(0f, _player.transform.rotation.y, 0f);
    }

    public void ChangeMap()
    {
        MapManager.Instance.ChangeMap(_changeMap);
    }

    private void PlayerLock(bool islock)
    {
        GameManager.Instance.PlayerLock(islock);
    }

    private void ColliderControl(bool enabled)
    {
        _boxCollider.enabled = enabled;
    }

    private PlayableDirector GetTimeLine(TimeLineType type)
    {
        var timeLine = TimeLineManager.Instance.GetTimeLine(type);

        if (timeLine != null)
        {
            return timeLine;
        }
        else
        {
            Debug.Log("<CutSceneDoor> 타임라인을 가져오지 못했습니다.");
            return null;
        }
    }

    private void BindTimeLine(TimeLineType type)
    {
        var timeLine = GetTimeLine(type);

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
