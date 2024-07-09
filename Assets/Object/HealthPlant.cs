using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPlant : MonoBehaviour, IInteractionItem
{
    private bool isUse;
    private bool isGrow;
    private Animator m_healthPlantAnim;
    private PlayerHealth m_playerHp;

    private void OnEnable()
    {
        isUse = false;
        isGrow = false;

        m_healthPlantAnim = GetComponent<Animator>();
    }
    public void InteractionItem(bool isAddItem)
    {
        if (isAddItem && !isGrow && InventoryManager.Instance.UseHealthItem())
        {
            isGrow = true;

            m_healthPlantAnim.SetBool("Grow", true);

            return;
        }

        if(isAddItem && !isUse && isGrow)
        {
            isUse = true;
            
            m_healthPlantAnim.SetBool("Grow", false);

            m_playerHp.PlayerHP = 4;

            UIManager.Instance.HideItemInteractionUI(gameObject.transform, InteractionUI_Type.Use);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(gameObject.transform, InteractionUI_Type.Use);

            m_playerHp = other.gameObject.GetComponent<PlayerHealth>(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(gameObject.transform, InteractionUI_Type.Use);
        }
    }

    
}
