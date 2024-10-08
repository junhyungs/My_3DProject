using Cinemachine;
using System.Collections;
using UnityEngine;

public class _NPC : MonoBehaviour
{
    [Header("NPC_Camera")]
    [SerializeField] private GameObject _npcCamaraObject;
    [Header("NPC_Name")]
    [SerializeField] private NPC _npcName;

    protected bool _story;

    protected IEnumerator CinemachineBlending(_NPC currentNPC)
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

        float time = brain.m_DefaultBlend.m_Time;

        yield return new WaitForSeconds(time);

        DialogueManager.Instance.StartDialogue(currentNPC, _npcName, DialogueOrder.Story);
    }

    public void ToggleNPC(bool toggle)
    {
        if(_npcCamaraObject != null)
        {
            _npcCamaraObject.SetActive(toggle);
        }

        if (toggle)
        {
            UIManager.Instance.HideItemInteractionUI(transform, ObjectName.InteractionDialogueUI);
        }
    }
}
