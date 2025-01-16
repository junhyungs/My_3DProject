using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image _loadIcon;

    private Coroutine _loadingCoroutine;
    private GameObject _childObject;

    private float _duration = 0.3f;
    private float _maxDuration = 2.0f;
    private float _exitDuration = 2.0f;

    private void Awake()
    {
        _childObject = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        UIManager._loadingUI += StartLoadingUI;
        UIManager.Instance.AddExitEvent(ExitImageBlink, true);
    }

    private void OnDisable()
    {
        UIManager._loadingUI -= StartLoadingUI;
        UIManager.Instance.AddExitEvent(ExitImageBlink, false);
    }

    public void StartLoadingUI(bool isStart)
    {
        if (isStart)
        {
            _childObject.SetActive(true);

            _loadingCoroutine = StartCoroutine(ImageBlink());
        }
        else
        {
            if(_loadingCoroutine != null)
            {
                StopCoroutine(_loadingCoroutine);

                _childObject.SetActive(false);

                _loadingCoroutine = null;
            }
        }
    }

    private IEnumerator ExitImageBlink()
    {
        _childObject.SetActive(true);

        Color color = _loadIcon.color;

        float durationTime = 0f;

        while (durationTime < _exitDuration)
        {
            yield return StartCoroutine(FadeImage(color, 1f, _duration));

            yield return StartCoroutine(FadeImage(color, 0f, _duration));

            durationTime += 2 * _duration;
        }

        _childObject.SetActive(false);
    }

    public IEnumerator ImageBlink()
    {
        Color color = _loadIcon.color;

        float durationTime = 0f;

        while (durationTime < _maxDuration)
        {
            yield return StartCoroutine(FadeImage(color, 1f, _duration));

            yield return StartCoroutine(FadeImage(color, 0f, _duration));

            durationTime += 2 * _duration;
        }
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
