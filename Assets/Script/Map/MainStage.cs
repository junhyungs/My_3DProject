using UnityEngine;

public class MainStage : Stage
{
    private void Start()
    {
        var startTimeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.Intro);

        if (startTimeLine != null)
        {
            startTimeLine.Play();
        }
    }

    public override void SpawnItems()
    {
        base.SpawnItems();
    }

    public override void CreateMonsters()
    {
        base.CreateMonsters();
    }

    public override void StartPosition()
    {
        base.StartPosition();
    }
}
