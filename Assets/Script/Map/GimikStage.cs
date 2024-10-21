using UnityEngine;

public class GimikStage : Stage
{
    [Header("SpawnGimik")]
    [SerializeField] private SpawnMonster _spawnGimikComponent;

    [Header("Door")]
    [SerializeField] private CutSceneDoor _door;

    private void OnEnable()
    {
        if(_door != null)
        {
            _door.CloseDoor();
        }
    }

    public override void SetMapData(MapData data)
    {
        base.SetMapData(data);

        if(_spawnGimikComponent == null)
        {
            return;
        }

        _spawnGimikComponent.Data = data;
    }

    public override void SpawnItems()
    {
        base.SpawnItems();
    }
}
