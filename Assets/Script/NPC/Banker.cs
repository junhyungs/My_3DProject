using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banker : _NPC, IInteractionDialogue
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

        StartCoroutine(StartDialogue(DialogueOrder.Loop));
    }

    protected override IEnumerator StartDialogue(DialogueOrder order)
    {
        yield return StartCoroutine(DialogueManager.Instance.StartBankerDialogue(_npcName, order));

        UIManager.Instance.OnAbilityUI();

        UIManager.Instance.HideItemInteractionUI(transform, ObjectName.InteractionDialogueUI);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UIManager.Instance.InteractionDialogueUI(transform, _uiPosition);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _onTrigger = false;

            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.InteractionDialogueUI);
        }
    }
}