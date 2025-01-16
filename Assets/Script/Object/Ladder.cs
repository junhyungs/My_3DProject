using UnityEngine;

public class Ladder : MonoBehaviour, IInteractionItem, IInteractionLadder
{
    [Header("UI_Position")]
    [SerializeField] private Vector3 _uiPosition;

    private float _lowestPointY;
    private float _highestPointY;

    private void Start()
    {
        var boxCollider = GetComponent<BoxCollider>();
        var boxCenter = boxCollider.center;


        var lowestPointY = boxCenter.y - (boxCollider.size.y / 2f);
        var highestPointY = boxCenter.y + (boxCollider.size.y / 2f);
        
        var worldLowestPointY = transform.TransformPoint(new Vector3(0f, lowestPointY, 0f));
        var worldHighestPointY = transform.TransformPoint(new Vector3(0f, highestPointY, 0f));

        _lowestPointY = worldLowestPointY.y;
        _highestPointY = worldHighestPointY.y;
    }

    public void InteractionItem()
    {
        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.LadderUI);
    }

    public void InteractionLadder(GameObject playerObject)
    {
        playerObject.transform.SetParent(transform);
        playerObject.transform.localPosition = Vector3.zero;
        playerObject.transform.rotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(transform, _uiPosition, ObjectName.LadderUI);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("LadderPlayer"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.LadderUI);
        }
    }

    public (float, float) LadderLength()
    {
        var ladder = (_lowestPointY,  _highestPointY);

        return ladder;
    }
}
