using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineManager : Singleton<TimeLineManager>
{
    [System.Serializable]
    public class TimeLine
    {
        [Header("TimeLineName")]
        public TimeLineType _timeLineType;
        [Header("TimeLine")]
        public PlayableDirector _director;
    }

    [Header("TimeLineList")]
    [SerializeField] private List<TimeLine> _timeLines;

    public PlayableDirector GetTimeLine(TimeLineType type)
    {
        foreach(var timeline in  _timeLines)
        {
            if(timeline._timeLineType == type && timeline._director != null)
            {
                return timeline._director;
            }
        }
        
        return null;
    }
}
