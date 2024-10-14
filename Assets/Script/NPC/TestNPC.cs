using System.Collections;
using UnityEngine;

public class TestNPC : _NPC, IInteractionDialogue
{
    [Header("NPC_Camera")]
    [SerializeField] private GameObject _npcCamaraObject;

    [Header("NPC_Name")]
    [SerializeField] private NPC _npcName;

    [Header("UIPosition")]
    [SerializeField] private Vector3 _uiPosition;

    public void TriggerDialogue()
    {
        if (_onTrigger)
        {
            return;
        }

        _onTrigger = true;

        ToggleNPC(true, _npcCamaraObject);

        if (_story)
        {
            _story = false;

            StartCoroutine(StartDialogue(DialogueOrder.Story));
        }
        else
        {
            StartCoroutine(StartDialogue(DialogueOrder.Loop));
        }
    }

    protected override IEnumerator StartDialogue(DialogueOrder order)
    {
        yield return CinemachineBlending();

        yield return StartCoroutine(DialogueManager.Instance.StartNormalNPC_Dialogue(_npcName, order));

        ToggleNPC(false, _npcCamaraObject);
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
