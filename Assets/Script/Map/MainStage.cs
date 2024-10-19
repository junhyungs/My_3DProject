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

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void SetMapData(MapData data)
    {
        base.SetMapData(data);
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
