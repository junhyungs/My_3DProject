using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingDoor : MonoBehaviour, IInteractionItem
{
    [Header("UI_Position")]
    [SerializeField] private Vector3 _uiPosition;

    [Header("InTransform")]
    [SerializeField] private Transform _inTransform;

    public Vector3 UIPosition => _uiPosition;

    private ParticleSystem _doorPayticle;
    private PlayableDirector _doorDirector;
    private Animator _animator;

    private HashSet<string> _track;

    private readonly int _drop = Animator.StringToHash("Drop");

    private void Awake()
    {
        _doorPayticle = gameObject.GetComponent<ParticleSystem>();

        _doorDirector = gameObject.GetComponent<PlayableDirector>();

        _animator = gameObject.GetComponent<Animator>();

        _track = new HashSet<string>
        {
            "PlayerWalk",
            "PlayerMove",
            "PlayerObject"
        };
    }

    private void OnEnable()
    {
        if(_doorPayticle != null)
        {
            _animator.SetTrigger(_drop);

            _doorPayticle.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
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

    public void InteractionItem()
    {
        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.OpenUI);

        GameObject player = GameManager.Instance.Player;

        ResetPlayerTransform(player);

        var track = _doorDirector.playableAsset.outputs;

        foreach(var output in track)
        {
            if (_track.Contains(output.streamName))
            {
                _doorDirector.SetGenericBinding(output.sourceObject, player);
            }
        }

        _doorDirector.Play();
    }

    private void ResetPlayerTransform(GameObject player)
    {
        player.transform.position = _inTransform.position;

        Vector3 rotateDirection = transform.position - player.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);

        player.transform.rotation = targetRotation;

    }

    public void ChangeMap()
    {
        MapManager.Instance.ChangeMap(Map.EndingStage);
    }
}
