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

        StartCoroutine(StartIntroDialogue(timeLine));
    }

    private IEnumerator StartIntroDialogue(PlayableDirector timeLine)
    {
        yield return StartCoroutine(DialogueManager.Instance.StartTimeLineDialogue(NPC.BusNPC, DialogueOrder.Story, timeLine));

        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

        float baseBlendTime = brain.m_DefaultBlend.m_Time;

        brain.m_DefaultBlend.m_Time = 8f;

        yield return new WaitForSeconds(brain.m_DefaultBlend.m_Time);

        brain.m_DefaultBlend.m_Time = baseBlendTime;
    }

    public void MainStage_HallCrow()
    {
     
    }

    public void OnActivePlayer()
    {
        GameManager.Instance.Player.SetActive(true);
    }
}
