using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class PlayerUI : MonoBehaviour
{
    [Header("ChildImageConponent")]
    [SerializeField] private List<Image> _childImageList;
    private RectTransform _uiRectTransform;

    private Vector2 _movePosition = new Vector2(0, 300f);
    private Vector2 _currentPosition;
    
    private float _moveDuration = 1f;
    private float _fadeDuration = 1f;

    private void Awake()
    {
        Transform childTransform = transform.GetChild(0);

        _uiRectTransform = childTransform.GetComponent<RectTransform>();

        _currentPosition = _uiRectTransform.anchoredPosition;
    }

    public void MovePlayerUI(bool ismove)
    {
        Action moveAction = ismove ? Up : Down;

        moveAction.Invoke();
    }

    private void Up()
    {
        Vector2 targetPosition = _uiRectTransform.anchoredPosition + _movePosition;

        if (IsMove(targetPosition))
        {
            _uiRectTransform.DOAnchorPos(targetPosition, _moveDuration).SetUpdate(true);

            ImageAlpaControl(false);
        }
    }

    private void Down()
    {
        Vector2 targetPosition = _currentPosition;

        if (IsMove(targetPosition))
        {
            _uiRectTransform.DOAnchorPos(targetPosition, _moveDuration).SetUpdate(true);

            ImageAlpaControl(true);
        }
    }

    private bool IsMove(Vector2 targetPosition)
    {
        if(_uiRectTransform.anchoredPosition == targetPosition)
        {
            return false;
        }

        return true;
    }

    private void ImageAlpaControl(bool alpaControl)
    {
        Action alpaAction = alpaControl ? IncreaseAlpa : DecreaseAlpa;  

        alpaAction.Invoke();
    }

    private void IncreaseAlpa()
    {
        foreach(var imageAlpa in _childImageList)
        {
            imageAlpa.DOFade(1f, _fadeDuration).SetUpdate(true);
        }
    }

    private void DecreaseAlpa()
    {
        foreach (var imageAlpa in _childImageList)
        {
            imageAlpa.DOFade(0f, _fadeDuration).SetUpdate(true);
        }
    }
}
