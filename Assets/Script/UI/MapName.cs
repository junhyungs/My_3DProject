using System.Collections;
using UnityEngine;
using TMPro;

public class MapName : MonoBehaviour
{
    [Header("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI[] _nameText;

    private Color[] _textColorArray;

    private void Awake()
    {
        _textColorArray = new Color[_nameText.Length];
    }

    private void OnEnable()
    {
        UIManager._onMapNameUI += OnNameText;
    }

    private void OnDisable()
    {
        UIManager._onMapNameUI -= OnNameText;
    }

    private void ResetText()
    {
        foreach(var nametext in _nameText)
        {
            nametext.text = string.Empty;
        }
    }

    public void OnNameText()
    {
        ResetText();

        var currentMapName = MapManager.Instance.CurrentMapName;

        var currentMapData = MapManager.Instance.GetMapData(currentMapName.ToString());

        if (currentMapData != null)
        {
            for (int i = 0; i < currentMapData.MapName.Count; i++)
            {
                _nameText[i].text = currentMapData.MapName[i];

                _textColorArray[i] = _nameText[i].color;
            }
        }

        StartCoroutine(TriggerName());
    }

    private IEnumerator TriggerName()
    {
        yield return StartCoroutine(FadeImage(_textColorArray, 1f, 1f));

        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(FadeImage(_textColorArray, 0f, 1f));
    }

    private IEnumerator FadeImage(Color[] color, float targetAlpha, float duration)
    {
        float[] startAlpha = new float[color.Length];

        float elapsed = 0f;

        for(int i = 0; i < color.Length; i++)
        {
            startAlpha[i] = color[i].a;
        }

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;

            for(int i = 0; i <  color.Length; i++)
            {
                color[i].a = Mathf.Lerp(startAlpha[i], targetAlpha, elapsed / duration);

                _nameText[i].color = color[i];
            }

            yield return null;
        }

        for(int i = 0; i < color.Length; i++)
        {
            color[i].a = targetAlpha;

            _nameText[i].color = color[i];
        }
    }

}
