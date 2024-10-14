using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrinketPanel : MonoBehaviour
{
    [Header("NavigateAction")]
    [SerializeField] private InputActionReference _navigateAction;

    [Header("PanelRectTransform")]
    [SerializeField] private RectTransform _panelRectTransform;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionName;

    [Header("GridLayOut")]
    [SerializeField] private GridLayoutGroup _layoutGroup;

    [Header("Slot")]
    [SerializeField] private TrinketSlot[] _slotArray;

    private List<Button> _buttonObjects;

    private Dictionary<GameObject, TrinketSlot> _slotDictioary;
    private TrinketSlot _currentSlot;

    private void Awake()
    {
        GetButtonComponent();

        _slotDictioary = new Dictionary<GameObject, TrinketSlot>();

        for(int i = 0; i < _buttonObjects.Count; i++)
        {
            GameObject slotObject = _buttonObjects[i].gameObject;

            TrinketSlot slotComponent = _slotArray[i];

            _slotDictioary.Add(slotObject, slotComponent);
        }
    }

    private void OnEnable()
    {
        OnEnableTrinketPanel();
    }

    private void OnEnableTrinketPanel()
    {
        PerformedAction(true);

        EventSystem.current.SetSelectedGameObject(_buttonObjects[0].gameObject);

        RefreshTrinketPanel(_buttonObjects[0].gameObject);
    }

    private void OnDisable()
    {
        PerformedAction(false);

        if(_currentSlot != null)
        {
            _currentSlot.CaptureImage();

            _currentSlot.InvokeEvent(false);
        }
    }

    private void PerformedAction(bool onEnable)
    {
        if (onEnable)
        {
            _navigateAction.action.Enable();

            _navigateAction.action.performed += SetActiveDescription;

            return;
        }

        _navigateAction.action.performed -= SetActiveDescription;
    }

    #region InitializeTrinketPanel
    private void GetButtonComponent()
    {
        if(_buttonObjects != null)
        {
            return;
        }

        _buttonObjects = new List<Button>();

        foreach(RectTransform childRectTransform in _panelRectTransform)
        {
            Button buttonComponent = childRectTransform.gameObject.GetComponent<Button>();

            _buttonObjects.Add(buttonComponent);
        }

        SetButtonArray();
    }

    private void SetButtonArray()
    {
        int panelWidth = (int)_panelRectTransform.rect.width;
        int panelHeight = (int)_panelRectTransform.rect.height;

        int slotWidth = (int)_layoutGroup.cellSize.x;
        int slotHeight = (int)_layoutGroup.cellSize.y;

        int row = panelWidth / slotWidth;   
        int column = panelHeight / slotHeight;

        Button[,] buttonArray = new Button[column, row];

        int listIndex = 0;

        for(int i = 0; i < buttonArray.GetLength(0); i++)
        {
            for(int k = 0; k < buttonArray.GetLength(1); k++)
            {
                buttonArray[i, k] = _buttonObjects[listIndex];

                listIndex++;
            }
        }

        BindNavigation(buttonArray);
    }

    private void BindNavigation(Button[,] buttonArray)
    {
        Navigation buttonNavigation;

        for(int i = 0; i < buttonArray.GetLength(0); i++)
        {
            for(int k = 0; k < buttonArray.GetLength(1); k++)
            {
                buttonNavigation = buttonArray[i, k].navigation;

                buttonNavigation.mode = Navigation.Mode.Explicit;

                if(i > 0)
                {
                    buttonNavigation.selectOnUp = buttonArray[i - 1, k];
                }
                else
                {
                    buttonNavigation.selectOnUp = null;
                }

                if(i < buttonArray.GetLength(0) - 1)
                {
                    buttonNavigation.selectOnDown = buttonArray[i + 1, k];
                }
                else
                {
                    buttonNavigation.selectOnDown = null;
                }

                if(k < buttonArray.GetLength(1) - 1)
                {
                    buttonNavigation.selectOnRight = buttonArray[i, k + 1];
                }
                else
                {
                    buttonNavigation.selectOnRight = null;
                }

                if(k > 0)
                {
                    buttonNavigation.selectOnLeft = buttonArray[i, k - 1];
                }
                else
                {
                    buttonNavigation.selectOnLeft = null;
                }

                buttonArray[i, k].navigation = buttonNavigation;
            }
        }
    }
    #endregion

    private void SetActiveDescription(InputAction.CallbackContext context)
    {
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;

        RefreshTrinketPanel(selectObject);
    }

    //비활성화 상태에서 호출되는 메서드.
    public void SetTrinketType(TrinketItemType type, ItemData data)
    {
        for(int i = 0; i < _slotArray.Length; i++)
        {
            if (_slotArray[i].Type == type &&!_slotArray[i].OnSlot)
            {
                _slotArray[i].OnSlot = true;

                _slotArray[i].Data = data;

                return;
            }
        }
    }

    private void ResetText()
    {
        if(_currentSlot != null)
        {
            _currentSlot.CaptureImage();

            _currentSlot.InvokeEvent(false);
        }

        _descriptionText.text = string.Empty;

        _descriptionName.text = string.Empty;
    }

    private TrinketSlot GetSlotComponent(GameObject selectObject)
    {
        if (!_slotDictioary.ContainsKey(selectObject))
        {
            Debug.Log("슬롯 컴포넌트를 반환하지 못했습니다.");
            return null;
        }

        return _slotDictioary[selectObject];
    }

    private void RefreshTrinketPanel(GameObject selectObject)
    {
        ResetText();

        var slotComponent = GetSlotComponent(selectObject);

        if (!slotComponent.OnSlot)
        {
            return;
        }

        slotComponent.LiveImage();

        var data = slotComponent.Data;

        _descriptionText.text = data.Description;

        _descriptionName.text = data.ItemName;

        _currentSlot = slotComponent;

        slotComponent.InvokeEvent(true);
    }
}
