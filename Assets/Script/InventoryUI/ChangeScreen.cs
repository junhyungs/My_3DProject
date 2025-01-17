using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ChangeScreen : MonoBehaviour
{
    private enum ScreenSize
    {
        Width,
        Height
    }

    [Header("ScreenSize ->EX)1920 X 1080")]
    [SerializeField] private string[] _screenSizeStringArray;

    private Resolution[] _screenSizeArray;

    private TMP_Dropdown _dropDown;

    private void Awake()
    {
        _dropDown = GetComponent<TMP_Dropdown>();

        ParseScreenSize();
        SetDropDownList();
    }

    private void OnEnable()
    {
        _dropDown.onValueChanged.AddListener(OnValueChanged_ScreenSize);
    }

    private void OnDisable()
    {
        _dropDown.onValueChanged.RemoveListener(OnValueChanged_ScreenSize);
    }

    private void Start()
    {
        SearchCurrentResolution();
    }

    private void ParseScreenSize()
    {
        _screenSizeArray = new Resolution[_screenSizeStringArray.Length];

        string splitString = " X ";

        for(int i = 0; i < _screenSizeStringArray.Length; i++)
        {
            string[] splitArray = _screenSizeStringArray[i].Split(splitString);

            SetResolution(i, splitArray);
        }
    }

    private void SetResolution(int index, string[] splitArray)
    {
        _screenSizeArray[index].width = ParseInt(splitArray[(int)ScreenSize.Width]);
        _screenSizeArray[index].height = ParseInt(splitArray[(int)ScreenSize.Height]);
    }

    private int ParseInt(string value)
    {
        if(int.TryParse(value, out int result))
        {
            return result;
        }

        Debug.Log("return 0");
        return 0;
    }

    private void SetDropDownList()
    {
        _dropDown.ClearOptions();

        var dropDownList = _screenSizeStringArray.ToList();

        _dropDown.AddOptions(dropDownList);
    }

    private void SearchCurrentResolution()
    {
        int currentScreenWidth = Screen.width;
        int currentScreenHeight = Screen.height;

        string findText = $"{currentScreenWidth} X {currentScreenHeight}";
        
        int findIndex = 0;

        for (int i = 0; i < _dropDown.options.Count; i++)
        {
            if (_dropDown.options[i].text == findText)
            {
                findIndex = i;
            }
        }
        
        if(findIndex >= 0)
        {
            _dropDown.value = findIndex;
        }
    }

    private void OnValueChanged_ScreenSize(int index)
    {
        var resolution = _screenSizeArray[index];

        Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.Windowed);
    }
}
