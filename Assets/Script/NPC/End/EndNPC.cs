using System.Collections;
using UnityEngine;

public class EndNPC : _NPC, IInteractionDialogue
{
    [Header("NPC_Name")]
    [SerializeField] private NPC _npcName;

    [Header("UI_Position")]
    [SerializeField] private Vector3 _uiPosition;

    public void TriggerDialogue()
    {
        if (_onTrigger)
        {
            return;
        }

        GameManager.Instance.PlayerLock(true);

        _onTrigger = true;

        StartCoroutine(StartDialogue(DialogueOrder.End));
    }

    protected override IEnumerator StartDialogue(DialogueOrder order)
    {
        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.ThankYouUI);

        yield return StartCoroutine(DialogueManager.Instance.StartNormalNPC_Dialogue(_npcName, order));

        GameManager.Instance.PlayerLock(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.ItemInteractionUI(transform, _uiPosition, ObjectName.ThankYouUI);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.ThankYouUI); 
        }
    }
}
