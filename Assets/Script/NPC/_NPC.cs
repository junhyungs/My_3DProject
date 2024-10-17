using Cinemachine;
using System.Collections;
using UnityEngine;

public abstract class _NPC : MonoBehaviour
{    
    protected bool _story = true;
    protected bool _onTrigger = false;


    protected abstract IEnumerator StartDialogue(DialogueOrder order);

    protected IEnumerator CinemachineBlending()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

        float time = brain.m_DefaultBlend.m_Time;

        yield return new WaitForSeconds(time);
    }

    public void ToggleNPC(bool toggle, GameObject npcCamera)
    {
        GameManager.Instance.PlayerLock(toggle);

        if (npcCamera != null)
        {
            npcCamera.SetActive(toggle);
        }

        if (toggle)
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.InteractionDialogueUI);
        }
    }
}
