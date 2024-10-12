using UnityEngine;

public class HealthItem : MonoBehaviour, IInteractionItem
{
    public void InteractionItem()
    {
        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);
        InventoryManager.Instance.SetHealthCount(1);
        gameObject.SetActive(false);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(transform, ObjectName.GetUI);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.GetUI);
        }
    }
}
