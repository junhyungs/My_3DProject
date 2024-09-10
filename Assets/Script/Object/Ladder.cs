using UnityEngine;

public class Ladder : MonoBehaviour, IInteractionItem
{
    public void InteractionItem(bool isAddItem)
    {
        if (isAddItem)
        {
            UIManager.Instance.HideItemInteractionUI(transform, InteractionUI_Type.Ladder);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(transform, InteractionUI_Type.Ladder);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, InteractionUI_Type.Ladder);
        }
    }
}
