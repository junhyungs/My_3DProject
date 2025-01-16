using System;
using UnityEngine;

public class InitializeImage : MonoBehaviour
{
    private GameObject _initializeImage;

    private void Awake()
    {
        _initializeImage = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        UIManager._initializeUI += ImageControl;
    }

    private void ImageControl(bool isActive)
    {
        Action imageAction = isActive ? OnEnableImage : OnDisableImage;

        imageAction.Invoke();
    }

    private void OnEnableImage()
    {
        _initializeImage.SetActive(true);
    }

    private void OnDisableImage()
    {
        _initializeImage.SetActive(false);

        UIManager._initializeUI -= ImageControl;
    }
}
