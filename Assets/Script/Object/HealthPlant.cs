using UnityEngine;

public class HealthPlant : MonoBehaviour, IInteractionItem
{
    [Header("UI_Position")]
    [SerializeField] private Vector3 _uiPosition;

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
    public void InteractionItem()
    {
        if (!isGrow && InventoryManager.Instance.UseHealthItem())
        {
            isGrow = true;

            m_healthPlantAnim.SetBool("Grow", true);

            return;
        }

        if(!isUse && isGrow)
        {
            isUse = true;
            
            m_healthPlantAnim.SetBool("Grow", false);

            m_playerHp.PlayerHP = 4;

            UIManager.Instance.HideItemInteractionUI(gameObject.transform, ObjectName.UseUI);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(gameObject.transform, _uiPosition, ObjectName.UseUI);

            m_playerHp = other.gameObject.GetComponent<PlayerHealth>(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(gameObject.transform, ObjectName.UseUI);
        }
    }

    
}
