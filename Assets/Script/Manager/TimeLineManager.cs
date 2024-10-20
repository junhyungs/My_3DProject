using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineManager : Singleton<TimeLineManager>
{
    private Dictionary<TimeLineType, PlayableDirector> _timeLineReference;

    private void Awake()
    {
        _timeLineReference = new Dictionary<TimeLineType, PlayableDirector>();
    }

    public void RegisterTimeLine(TimeLineType type, PlayableDirector director)
    {
        if(_timeLineReference.ContainsKey(type) || director == null)
        {
            return;
        }

        _timeLineReference.Add(type, director);
    }

    public PlayableDirector GetTimeLine(TimeLineType type)
    {
        foreach(var timeLine in _timeLineReference)
        {
            if(timeLine.Key == type)
            {
                return timeLine.Value;  
            }
        }

        Debug.Log("타임라인 참조 리턴에 실패했습니다.");
        return null;    
    }
}
