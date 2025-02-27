using UnityEngine;

public class EndStage : Stage
{
    [Header("StartPosition")]
    [SerializeField] private Transform _startTransform;

    private void OnEnable()
    {
        GameObject player = GameManager.Instance.Player;

        player.transform.position = _startTransform.position;
        player.transform.rotation = _startTransform.rotation;

        player.SetActive(true);

        StartCoroutine(StopLoadingUI());
    }
}
