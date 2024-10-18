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

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _internalDoor.SetActive(false);
    }

    public void InteractionItem()
    {
        _boxCollider.enabled = false;

        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);

        GameManager.Instance.PlayerLock(true);

        ResetPlayerTransform();

        var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.Move);

        var trak = timeLine.playableAsset.outputs;

        foreach(var output in trak)
        {
            if(output.streamName == "PlayerMovement" ||
                output.streamName == "PlayerWalkAnimation" ||
                output.streamName == "PlayerObject")
            {
                timeLine.SetGenericBinding(output.sourceObject, _player);
            }
        }

        timeLine.Play();
    }

    private void ResetPlayerTransform()
    {
        _player.transform.SetParent(transform);

        _player.transform.localPosition = _movementTransform.localPosition;

        _player.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        _player.transform.parent = null;
    }

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

            UIManager.Instance.ItemInteractionUI(transform, _uiPosition, ObjectName.GetUI);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);
        }
    }
}
