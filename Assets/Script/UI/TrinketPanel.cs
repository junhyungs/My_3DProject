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
    [Header("TrinketSlotPanel")]
    [SerializeField] private RectTransform _panelRectTransform;
    [Header("ChildRectTransform")]
    [SerializeField] private RectTransform _childRectTransform;
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private List<Button> _buttonObjects;

    private int _listIndex = 0;

    private void OnEnable()
    {
        _navigateAction.action.Enable();

        _navigateAction.action.performed += SetActiveChildImage;

        InitializeButtonObject();

        EventSystem.current.SetSelectedGameObject(_buttonObjects[0].gameObject);
    }

    private void InitializeButtonObject()
    {
        if (_buttonObjects == null)
        {
            _buttonObjects = new List<Button>();

            foreach (RectTransform childTransform in _panelRectTransform)
            {
                Button childButton = childTransform.gameObject.GetComponent<Button>();

                _buttonObjects.Add(childButton);
            }
            Debug.Log(_buttonObjects.Count);

            int parentWidth = (int)_panelRectTransform.rect.width;
            int parentHeight = (int)_panelRectTransform.rect.height;

            int childWidth = 150;
            int childHeight = 150;

            int widthCount = parentWidth / childWidth;
            int heightCount = parentHeight / childHeight;

            Button[,] buttonArray = new Button[heightCount, widthCount];

            Navigation currentNavigation;

            int listIndex = 0;

            for (int i = 0; i < buttonArray.GetLength(0); i++)
            {
                for (int k = 0; k < buttonArray.GetLength(1); k++)
                {
                    buttonArray[i,k] = _buttonObjects[listIndex];

                    currentNavigation = buttonArray[i, k].navigation;

                    currentNavigation.mode = Navigation.Mode.Explicit;

                    if(i > 0)
                    {
                        currentNavigation.selectOnUp = buttonArray[i - 1, k];
                    }
                    else
                    {
                        currentNavigation.selectOnUp = null;
                    }

                    if(i < heightCount - 1)
                    {
                        currentNavigation.selectOnDown = buttonArray[i + 1, k];
                    }
                    else
                    {
                        currentNavigation.selectOnDown = null;
                    }

                    if(k < widthCount - 1)
                    {
                        currentNavigation.selectOnRight = buttonArray[i, k + 1];
                    }
                    else
                    {
                        currentNavigation.selectOnRight = null;
                    }

                    if(k > 0)
                    {
                        currentNavigation.selectOnLeft = buttonArray[i, k - 1];
                    }
                    else
                    {
                        currentNavigation.selectOnLeft = null;
                    }

                    buttonArray[i, k].navigation = currentNavigation;

                    listIndex++;
                }
            }
        }
    }

    private void SetActiveChildImage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject);
        }
    }

    


}
