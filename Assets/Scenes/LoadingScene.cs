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

        //false�� �Ǹ� ���� 90%������ �ҷ����� �ȴ�. 
        //true�� �Ǿ ��������� �ʹ� ���� ����ȯ���� ���� �������� �ε����� �ҿ���� �� �� ����.
        //�̷����� �����ϰ��� false�� �ξ� 90%������ �ε��� �� �� �ٸ� �߿��� �۾��� �ϴ°�ó�� �ε����� ��� ����ũ ����� �ִ°�.
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
        float duration = 0.5f; // �����̴� �ӵ�
        Color color = loadImage.color;

        // �ε� �ð� ���� �����̱� (���� ��� 5�� ���� �����̰� ����)
        float totalBlinkTime = 5f;
        float elapsedBlinkTime = 0f;

        while (elapsedBlinkTime < totalBlinkTime)
        {
            // ���İ��� 1�� ������ ����
            yield return StartCoroutine(FadeImage(color, 1f, duration));
            // ���İ��� 0���� ������ ����
            yield return StartCoroutine(FadeImage(color, 0f, duration));

            elapsedBlinkTime += duration * 2;
        }

        // �ε��� �Ϸ�Ǿ��ٰ� �����ϰ� isLoad�� true�� ����
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

