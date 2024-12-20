using UnityEngine;

public class ResetInventoryUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject[] _panels;

    public void DisablePanelUI()
    {
        foreach(var panel in _panels)
        {
            panel.SetActive(false);
        }
    }
}
