using UnityEngine;

public class MapNameTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.OnMapNameUI();

            gameObject.SetActive(false);    
        }
    }
}
