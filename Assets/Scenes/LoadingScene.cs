using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    private Image loadImage;

    private bool isLoad = false;

    private void OnEnable()
    {
        isLoad = false;
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        SceneManager.LoadScene("LoadScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
        StartCoroutine(ImageBlink());
    }

    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        //false가 되면 씬은 90%정도만 불러오게 된다. 
        //true가 되어도 상관없지만 너무 빠른 씬전환으로 인해 만들어놓은 로딩씬이 소용없게 될 수 있음.
        //이런것을 방지하고자 false로 두어 90%정도만 로딩을 한 후 다른 중요한 작업을 하는것처럼 로딩씬을 띄워 페이크 모션을 주는것.
        op.allowSceneActivation = false;

        //float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if(op.progress >= 0.9f && isLoad)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
        
    }

    private IEnumerator ImageBlink()
    {
        float duration = 0.5f; // 깜빡이는 속도
        Color color = loadImage.color;

        // 로딩 시간 동안 깜빡이기 (예를 들어 5초 동안 깜빡이게 설정)
        float totalBlinkTime = 5f;
        float elapsedBlinkTime = 0f;

        while (elapsedBlinkTime < totalBlinkTime)
        {
            // 알파값을 1로 서서히 증가
            yield return StartCoroutine(FadeImage(color, 1f, duration));
            // 알파값을 0으로 서서히 감소
            yield return StartCoroutine(FadeImage(color, 0f, duration));

            elapsedBlinkTime += duration * 2;
        }

        // 로딩이 완료되었다고 가정하고 isLoad를 true로 설정
        isLoad = true;
    }
    

    private IEnumerator FadeImage(Color color, float targetAlpha, float duration)
    {
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            loadImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        loadImage.color = color;
    }
}

