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

    private List<Button> _buttonObjects;

    private void OnEnable()
    {
        PerformedAction();
        GetButtonComponent();

        EventSystem.current.SetSelectedGameObject(_buttonObjects[0].gameObject);
    }

    private void PerformedAction()
    {
        _navigateAction.action.Enable();

        _navigateAction.action.performed += SetActiveDescription;
    }

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

        int slotWidth = 150;
        int slotHeight = 150;

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

    private void SetActiveDescription(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }
}
