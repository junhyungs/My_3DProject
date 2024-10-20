using UnityEngine;

public class GimikStage : Stage
{
    [Header("SpawnGimik")]
    [SerializeField] private SpawnMonster _spawnGimikComponent;

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
