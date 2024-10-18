using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TimeLineReference : PlayableAsset
{
    public ExposedReference<GameObject> _playerObject;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var newPlayable = ScriptPlayable<TimeLinePlayableAsset>.Create(graph);
        TimeLinePlayableAsset behaviour = newPlayable.GetBehaviour();

        behaviour.Player = _playerObject.Resolve(graph.GetResolver());

        return newPlayable;
    }
}


public class TimeLinePlayableAsset : PlayableBehaviour
{
    public GameObject Player { get; set; }

    

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        Player.SetActive(true);
    }
}
