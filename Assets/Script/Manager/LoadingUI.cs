using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image _loadIcon;

    private GameObject _childObject;

    private float _duration = 0.3f;

    private void Awake()
    {
        _childObject = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        UIManager._loadingUI += StartLoadingUI;
    }

    private void OnDisable()
    {
        UIManager._loadingUI -= StartLoadingUI;
    }

    public void StartLoadingUI(ResourceRequest request)
    {
        _childObject.SetActive(true);

        StartCoroutine(ImageBlink(request));
    }

    public IEnumerator ImageBlink(ResourceRequest request)
    {
        Color color = _loadIcon.color;

        while (!request.isDone)
        {
            yield return StartCoroutine(FadeImage(color, 1f, _duration));

            yield return StartCoroutine(FadeImage(color, 0f, _duration));
        }

        yield return StartCoroutine(FadeImage(color, 1f, _duration));

        yield return StartCoroutine(FadeImage(color, 0f, _duration));

        _childObject.SetActive(false);
    }

    private IEnumerator FadeImage(Color color, float targetAlpha, float duration)
    {
        float startAlpha = color.a;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);

            _loadIcon.color = color;

            yield return null;
        }

        color.a = targetAlpha;

        _loadIcon.color = color;
    }

}
