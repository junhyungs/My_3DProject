using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.Player = this.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteraction();
        }
    }

    private void OnInteraction()
    {
        Vector3 spherePosition = transform.position + new Vector3(0, 0.5f, 0);

        float sphereRadius = 0.5f;

        Collider[] colliders = Physics.OverlapSphere(spherePosition, sphereRadius);

        foreach(var collider in colliders)
        {
            IInteractionItem interaction = collider.gameObject.GetComponent<IInteractionItem>();

            if(interaction != null)
            {
                interaction.InteractionItem(true);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0,0.5f,0), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Soul")
        {
            DropSoul soul = other.gameObject.GetComponent<DropSoul>();

            int soulValue = soul.GetSoulValue();

            InventoryManager.Instance.SetSoulCount(soulValue);
        }

       
    }
}
