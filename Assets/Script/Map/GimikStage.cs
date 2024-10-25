using UnityEngine;

public class GimikStage : Stage
{
    [Header("SpawnGimik")]
    [SerializeField] private SpawnMonster _spawnGimikComponent;

    [Header("Door")]
    [SerializeField] private CutSceneDoor _door;
    [SerializeField] private Transform _outTransform;

    [Header("EndingDoor")]
    [SerializeField] private GameObject _endingDoor;

    private void OnEnable()
    {
        if(_door != null)
        {
            GameObject player = GameManager.Instance.Player;

            player.transform.position = _outTransform.position;

            _door.CloseDoor();
        }
    }

    private new void OnDisable()
    {
        _endingDoor.SetActive(false);
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
