using UnityEngine;

public class Ladder : MonoBehaviour, IInteractionItem
{
    [Header("UI_Position")]
    [SerializeField] private Vector3 _uiPosition;

    public void InteractionItem()
    {
        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.LadderUI);
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
}
