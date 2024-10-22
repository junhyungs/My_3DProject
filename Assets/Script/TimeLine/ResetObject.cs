using UnityEngine;
using UnityEngine.UI;

public class ResetObject : MonoBehaviour
{
    private enum ImageResetMode
    {
        AlphaZero,
        AlphaMax
    }

    [Header("Image")]
    [SerializeField] private Image _resetImage;
    [SerializeField] private ImageResetMode _imageResetMode;

    [Header("GameObject")]
    [SerializeField] private GameObject _resetObject;

    private void OnDisable()
    {
        ImageReset();
        GameObjectReset();
    }

    private void ImageReset()
    {
        if(_resetImage == null)
        {
            return;
        }

        Color color = _resetImage.color;

        if(_imageResetMode == ImageResetMode.AlphaZero)
        {
            color.a = 0f;
        }
        else if(_imageResetMode == ImageResetMode.AlphaMax)
        {
            color.a = 1f;
        }

        _resetImage.color = color;
    }

    private void GameObjectReset()
    {
        if(_resetObject == null)
        {
            return;
        }

        _resetObject.SetActive(false);
    }

}
