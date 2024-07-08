using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;




public class StartUI : MonoBehaviour
{
    [System.Serializable]
    public class ButtonData
    {
        public Button button;
        public GameObject leftImage;
        public GameObject rightImage;
        public Text buttonText;
        [HideInInspector] public Vector3 initLeftPosition;
        [HideInInspector] public Vector3 initRightPosition;

        public UnityEngine.Events.UnityEvent onClick;

    }

    public List<ButtonData> buttons;
    public float moveAmount = 10f;
    public float moveDuration = 0.5f;

    private int selectedIndex = 0;
    private List<Tweener> leftTweeners = new List<Tweener>();
    private List<Tweener> rightTweeners = new List<Tweener>();
    

    void Start()
    {
        foreach(var buttonData in buttons)
        {
            SetButtonTextColors(buttonData.buttonText, false);
            buttonData.leftImage.SetActive(false);
            buttonData.rightImage.SetActive(false);

            buttonData.initLeftPosition = buttonData.leftImage.transform.localPosition;
            buttonData.initRightPosition = buttonData.rightImage.transform.localPosition;

            leftTweeners.Add(null);
            rightTweeners.Add(null);
        }

        ActiveButtonImage(buttons[0], true);
        ChangeSelection(0);
        Debug.Log(buttons.Count);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            OnButton();
        }
    }

    private void ActiveButtonImage(ButtonData button, bool isActive)
    {
        button.leftImage.SetActive(isActive);
        button.rightImage.SetActive(isActive);
    }

    private void ChangeSelection(int direction)
    {
        if (buttons.Count == 0)
            return;

        int previousIndex = selectedIndex;

        selectedIndex += direction;

        if(selectedIndex < 0)
        {
            selectedIndex = buttons.Count - 1;
        }
        else if(selectedIndex > buttons.Count - 1)
        {
            selectedIndex = 0;
        }

        //Mathf.Clamp(value, min, max) value 값을 min, max로 제한하는 함수
        //selectedIndex = Mathf.Clamp(selectedIndex + direction, 0, buttons.Count -1);

        //if (selectedIndex == saveIndex)
        //    return;

        //saveIndex = selectedIndex;

        if (previousIndex != selectedIndex)
        {
            AnimateImages(buttons[previousIndex], false);
            SetButtonTextColors(buttons[previousIndex].buttonText, false);
            
        }

        AnimateImages(buttons[selectedIndex], true);
        SetButtonTextColors(buttons[selectedIndex].buttonText, true);
        
    }

    private void AnimateImages(ButtonData buttonData, bool toRight)
    {
        int index = buttons.IndexOf(buttonData);

        // 이전에 실행 중인 Tween 정지
        if (leftTweeners[index] != null)
        {
            leftTweeners[index].Kill();
            leftTweeners[index] = null;
        }
        if (rightTweeners[index] != null)
        {
            rightTweeners[index].Kill();
            rightTweeners[index] = null;
        }

        buttonData.leftImage.transform.localPosition = buttonData.initLeftPosition;
        buttonData.rightImage.transform.localPosition = buttonData.initRightPosition;

        // 새로운 Tween 생성
        if (toRight)
        {
            float targetX_Left = buttonData.leftImage.transform.position.x - moveAmount;
            float targetX_Right = buttonData.rightImage.transform.position.x + moveAmount;

            leftTweeners[index] = buttonData.leftImage.transform.DOMoveX(targetX_Left, moveDuration).SetLoops(-1, LoopType.Yoyo);
            rightTweeners[index] = buttonData.rightImage.transform.DOMoveX(targetX_Right, moveDuration).SetLoops(-1, LoopType.Yoyo);
            ActiveButtonImage(buttons[index], true);
        }
        else
        {
            ActiveButtonImage(buttonData, false);
        }
        
    }

    private void SetButtonTextColors(Text buttonText, bool isSelected)
    {
        Color color = isSelected ? Color.white : Color.gray;

        buttonText.color = color;
    }

    private void OnButton()
    {
        if(buttons.Count > 0)
        {
            ButtonData currentButton = buttons[selectedIndex];

            currentButton.onClick.Invoke();
        }
    }

    public void OnNextPanel()
    {
        DOTween.KillAll();

        LoadingScene.LoadScene("R_Boss");
    }

    public void OnOptionPanel()
    {
        Debug.Log("Option");
    }

    public void OnExit()
    {
        Debug.Log("Exit");
    }

}
