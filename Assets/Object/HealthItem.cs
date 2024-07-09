using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour, IInteractionItem
{
    public void InteractionItem(bool isAddItem)
    {
        if(isAddItem)
        {
            UIManager.Instance.HideItemInteractionUI(transform, InteractionUI_Type.Get);
            InventoryManager.Instance.SetHealthCount(1);
            gameObject.SetActive(false);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(transform, InteractionUI_Type.Get);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, InteractionUI_Type.Get);
        }
    }
}
