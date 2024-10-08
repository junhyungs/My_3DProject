using UnityEngine;

public class TestNPC : _NPC, IInteractionDialogue
{
    public void TriggerDialogue()
    {
        if (_onTrigger)
        {
            return;
        }

        _onTrigger = true;

        ToggleNPC(true);

        if (_story)
        {
            _story = false;

            StartCoroutine(CinemachineBlending(this, DialogueOrder.Story));
        }
        else
        {
            StartCoroutine(CinemachineBlending(this, DialogueOrder.Loop));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(_player == null)
            {
                _player = other.GetComponent<Player>();
            }

            UIManager.Instance.InteractionDialogueUI(transform);
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
