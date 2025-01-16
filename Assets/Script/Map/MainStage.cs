using UnityEngine;

public class MainStage : Stage
{
    [Header("Door")]
    [SerializeField] private CutSceneDoor _door;

    [Header("StartPosition")]
    [SerializeField] private Transform _startTransform;

    private bool _isFirst = true;

    private void Awake()
    {
        RespawnPoint = _startTransform;
    }

    private void OnEnable()
    {
        if (!_isFirst)
        {
            StartCoroutine(StopLoadingUI(_door.CloseDoor));
        }
    }

    private void Start()
    {
        UIManager.Instance.OnLoadingUI(false);

        SetPlayer();

        var timeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.Intro);

        if(timeLine != null)
        {
            _isFirst = false;

            timeLine.Play();
        }
    }

    private void SetPlayer()
    {
        GameObject player = GameManager.Instance.Player;

        player.transform.position = _startTransform.position;

        player.transform.rotation = Quaternion.identity;
    }


    public override void SetMapData(MapData data)
    {
        base.SetMapData(data);
    }

    public override void SpawnItems()
    {
        base.SpawnItems();
    }
}
