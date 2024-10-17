using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SignalTrigger : MonoBehaviour
{
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

    }

    public void MainStage_HallCrow_3()
    {

    }

    public void MainStage_HallCrow_4()
    {

    }

    public void MainStage_HallCrow_5()
    {

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

    private void PlayerLock(bool isLock)
    {
        GameManager.Instance.PlayerLock(isLock);
    }
}
