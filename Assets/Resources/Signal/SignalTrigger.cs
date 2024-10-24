using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SignalTrigger : MonoBehaviour
{
    private TimeLinePlayer _dummyPlayer;

    private void Start()
    {
        _dummyPlayer = transform.GetComponentInChildren<TimeLinePlayer>(true);
    }

    #region MainStage
    public void MainStage_Intro()
    {
        var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.Intro);

        StartCoroutine(DialogueManager.Instance.StartTimeLineDialogue(NPC.BusNPC, DialogueOrder.Story, timeLine));
    }

    #region HallCrow_Dialogue
    public void MainStage_HallCrow()
    {
        var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.HallCrow);

        StartCoroutine(DialogueManager.Instance.StartTimeLineDialogue(NPC.HallCrow_1, DialogueOrder.Story, timeLine));
    }

    public void MainStage_HallCrow_2()
    {
        var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.HallCrow);

        StartCoroutine(DialogueManager.Instance.StartTimeLineDialogue(NPC.HallCrow_2, DialogueOrder.Story, timeLine));
    }

    public void MainStage_HallCrow_3()
    {
        var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.HallCrow);

        StartCoroutine(DialogueManager.Instance.StartTimeLineDialogue(NPC.HallCrow_3, DialogueOrder.Story, timeLine));  
    }

    public void MainStage_HallCrow_4()
    {
        var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.HallCrow);

        StartCoroutine(HallCrow_4(timeLine));
    }

    private IEnumerator HallCrow_4(PlayableDirector timeLine)
    {
        yield return StartCoroutine(DialogueManager.Instance.StartTimeLineDialogue(NPC.HallCrow_4, DialogueOrder.Story, timeLine));

        ResetPlayer();
    }

    private void ResetPlayer()
    {
        Transform dummyTransform = _dummyPlayer.gameObject.transform;

        Transform playerTransform = GameManager.Instance.Player.transform;

        playerTransform.position = dummyTransform.position;
        playerTransform.rotation = dummyTransform.rotation;

        _dummyPlayer.gameObject.SetActive(false);

        OnActivePlayer();
    }

    #endregion
    #endregion

    #region BossStage
    public void BossStageChangeMap_Gimik()
    {
        MapManager.Instance.ChangeMap(Map.GimikStage);
    }

    #endregion

    public void OnDeActivePlayer()
    {
        GameManager.Instance.Player.SetActive(false);
    }

    public void OnActivePlayer()
    {
        GameManager.Instance.Player.SetActive(true);
    }

    public void ReleasePlayer()
    {
        GameManager.Instance.PlayerLock(false);
    }

    public void LockPlayer()
    {
        GameManager.Instance.PlayerLock(true);
    }

}
