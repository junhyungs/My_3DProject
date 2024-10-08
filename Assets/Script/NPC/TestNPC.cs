using UnityEngine;

public class TestNPC : _NPC, IInteractionDialogue
{
    public void TriggerDialogue()
    {
        ToggleNPC(true);

        StartCoroutine(CinemachineBlending(this));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.InteractionDialogueUI(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.InteractionDialogueUI);
        }
    }
}
