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
            Debug.Log("데이터가 Name 딕셔너리에 없습니다.");
            return null;
        }

        string name = _nameDictionary[id];

        return name;
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

    #region Normal_NPC
    public IEnumerator StartNormalNPC_Dialogue(NPC npc, DialogueOrder order)
    {
        string id = npc.ToString();

        OnActorName(id);

        List<string> dialogueList = GetList(id, order);

        yield return StartCoroutine(ReadDialogue(dialogueList));
    }
    #endregion

    #region Banker
    public IEnumerator StartBankerDialogue(NPC npc, DialogueOrder order)
    {
        string id = npc.ToString();

        OnActorName(id);

        List<string> dialogueList = GetList(id, order);

        yield return StartCoroutine(ReadDialogue(dialogueList));
    }

    #endregion

    private void OnActorName(string id)
    {
        _dialogueName.text = string.Empty;

        _dialogueName.text = GetName(id);
    }

    private IEnumerator ReadDialogue(List<string> dialogueList)
    {
        _dialogueUI.gameObject.SetActive(true);

        _dialogueText.text = string.Empty;

        yield return _uiRect.DOScale(_maxScale, _durationTime).WaitForCompletion();

        foreach(var message in dialogueList)
        {
            yield return StartCoroutine(Message(message));

            yield return new WaitUntil(() =>
            {
                return Input.GetKeyDown(KeyCode.Return);
            });
        }

        yield return _uiRect.DOScale(_minScale, _durationTime).WaitForCompletion();

        _dialogueUI.gameObject.SetActive(false);
    }

    private IEnumerator Message(string message)
    {
        _dialogueText.text = string.Empty;

        foreach (char charMessage in message)
        {
            _dialogueText.text += charMessage;

            yield return new WaitForSeconds(0.05f);
        }
    }
}