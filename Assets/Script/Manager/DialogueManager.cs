using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class DialogueManager : Singleton<DialogueManager>
{
    [Header("DialogueUI")]
    [SerializeField] private Image _dialogueUI;

    [Header("DialogueName")]
    [SerializeField] private TextMeshProUGUI _dialogueName;

    [Header("DialogueText")]
    [SerializeField] private TextMeshProUGUI _dialogueText;

    #region DoTween
    private RectTransform _uiRect;
    private float _durationTime = 0.5f;
    private Vector3 _maxScale = new Vector3(1f,1f,1f);
    private Vector3 _minScale = Vector3.zero;
    #endregion

    private Dictionary<string, Dictionary<DialogueOrder, List<string>>> _dialogueDictionary
        = new Dictionary<string, Dictionary<DialogueOrder, List<string>>>();

    private Dictionary<string, string> _nameDictionary = new Dictionary<string, string>();
    
    void Start()
    {
       _uiRect = _dialogueUI.gameObject.GetComponent<RectTransform>();

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

                AddName(data);

                return AddData(data);
            });
        }
    }

    private void AddName(DialogueData data)
    {
        if (!_nameDictionary.ContainsKey(data.ID))
        {
            _nameDictionary.Add(data.ID, data.Name);
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

    private string GetName(string id)
    {
        if (!_nameDictionary.ContainsKey(id))
        {
            Debug.Log("�����Ͱ� Name ��ųʸ��� �����ϴ�.");
            return null;
        }

        string name = _nameDictionary[id];

        return name;
    }

    private List<string> GetList(string id, DialogueOrder order)
    {
        if (!_dialogueDictionary.ContainsKey(id))
        {
            Debug.Log("�����Ͱ� ���̾�α� ��ųʸ��� �����ϴ�.");
            return null;
        }
        
        Dictionary<DialogueOrder, List<string>> orderDictionary = _dialogueDictionary[id];

        List<string> dialogList = orderDictionary[order];

        return dialogList;
    }

    public void StartDialogue(_NPC currentNPC ,NPC npc, DialogueOrder order)
    {
        string id = npc.ToString();

        _dialogueName.text = string.Empty;

        _dialogueName.text = GetName(id);

        List<string> dialogList = GetList(id, order);

        StartCoroutine(ReadMessage(dialogList, currentNPC));
    }

    private IEnumerator ReadMessage(List<string> messageList, _NPC currentNPC)
    {
        _dialogueUI.gameObject.SetActive(true);

        _dialogueText.text = string.Empty;

        yield return _uiRect.DOScale(_maxScale, _durationTime).WaitForCompletion();

        foreach(var message in messageList)
        {
            yield return StartCoroutine(Message(message));

            yield return new WaitUntil(() =>
            {
                return Input.GetKeyDown(KeyCode.Return);
            });
        }

        yield return _uiRect.DOScale(_minScale, _durationTime).WaitForCompletion();

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