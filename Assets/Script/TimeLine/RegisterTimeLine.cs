using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RegisterTimeLine : MonoBehaviour
{
    [Header("TimeLineType")]
    [SerializeField] private TimeLineType _type;

    private PlayableDirector _timeLine;

    private void Awake()
    {
        _timeLine = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        TimeLineManager.Instance.RegisterTimeLine(_type, _timeLine);
    }
}
