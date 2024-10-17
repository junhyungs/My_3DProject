using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security : _NPC, IInteractionDialogue
{
    [Header("NPC_Name")]
    [SerializeField] private NPC _npcName;

    [Header("Order")]
    [SerializeField] private DialogueOrder _dialogueOrder;

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

        StartCoroutine(StartDialogue(_dialogueOrder));
    }

    protected override IEnumerator StartDialogue(DialogueOrder order)
    {
        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.InteractionDialogueUI);

        yield return StartCoroutine(DialogueManager.Instance.StartNormalNPC_Dialogue(_npcName, order));

        GameManager.Instance.PlayerLock(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.InteractionDialogueUI(transform, _uiPosition);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _onTrigger = false;

            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.InteractionDialogueUI);
        }
    }
}
