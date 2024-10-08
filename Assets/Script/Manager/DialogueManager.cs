using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("DialogueUI")]
    [SerializeField] private Image _dialogueUI;

    [Header("DialogueText")]
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private Dictionary<string, Dictionary<DialogueOrder, List<string>>> _dialogueDictionary
        = new Dictionary<string, Dictionary<DialogueOrder, List<string>>>();
    
    void Start()
    {
       StartCoroutine(LoadDialogueData());
    }

    private IEnumerator LoadDialogueData()
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(nameof(TestNPC)) == null;
        });

        Array enumArray = Enum.GetValues(typeof(NPC));

        for(int i = 0; i < enumArray.Length; i++)
        {
            yield return new WaitUntil(() =>
            {
                string id = enumArray.GetValue(i).ToString();

                var data = DataManager.Instance.GetData(id) as DialogueData;

                return AddData(data);
            });
        }
    }

    private bool AddData(DialogueData data)
    {
        if (!_dialogueDictionary.ContainsKey(data.ID))
        {
            Dictionary<DialogueOrder, List<string>> orderDictionary = new Dictionary<DialogueOrder, List<string>>
            {
                {DialogueOrder.Story, data.StoryMessage },
                {DialogueOrder.Loop, data.LoopMessage }
            };

            _dialogueDictionary.Add(data.ID, orderDictionary);

            return true;
        }

        return false;
    }

    private List<string> GetList(string id, DialogueOrder order)
    {
        if (!_dialogueDictionary.ContainsKey(id))
        {
            Debug.Log("데이터가 다이얼로그 딕셔너리에 없습니다.");
            return null;
        }
        
        Dictionary<DialogueOrder, List<string>> orderDictionary = _dialogueDictionary[id];

        List<string> dialogList = orderDictionary[order];

        return dialogList;
    }

    public void StartDialogue(_NPC currentNPC ,NPC npc, DialogueOrder order)
    {
        string id = npc.ToString();

        List<string> dialogList = GetList(id, order);

        StartCoroutine(ReadMessage(dialogList, currentNPC));
    }

    public void EndDialogue()
    {
        Debug.Log("끝남");
    }

    private IEnumerator ReadMessage(List<string> messageList, _NPC currentNPC)
    {
        _dialogueUI.gameObject.SetActive(true);

        _dialogueText.text = string.Empty;

        foreach(var message in messageList)
        {
            yield return StartCoroutine(Message(message));

            yield return new WaitUntil(() =>
            {
                return Input.GetKeyDown(KeyCode.Return);
            });
        }

        EndDialogue();

        currentNPC.ToggleNPC(false);

        _dialogueUI.gameObject.SetActive(false);
    }

    private IEnumerator Message(string message)
    {
        _dialogueText.text = string.Empty;

        foreach(char charMessage in message)
        {
            _dialogueText.text += charMessage;

            yield return new WaitForSeconds(0.05f);
        }
    }
}