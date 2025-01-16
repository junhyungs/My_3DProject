using UnityEngine;
using UnityEngine.UI;

public class TriggerButton : MonoBehaviour
{
    [Header("TriggerObject")]
    [SerializeField] private GameObject _uiObject;

    [Header("Image")]
    [SerializeField] private Image _image;

    private Color _currentColor;

    public void OnEnableUI()
    {
        _uiObject.SetActive(true);

        SetAlpha(1f);
    }

    public void OnDisableUI()
    {
        _uiObject.SetActive(false);

        SetAlpha(0.1f);
    }

    private void SetAlpha(float alphaValue)
    {
        _currentColor = _image.color;

        _currentColor.a = alphaValue;

        _image.color = _currentColor;
    }
}
