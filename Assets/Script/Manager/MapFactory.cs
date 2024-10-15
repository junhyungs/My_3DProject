using System.Collections;
using UnityEngine;

public class MapFactory
{
    private MonoBehaviour _monoBehaviour;

    public MapFactory(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void LoadMapPrefab(string prefabPath, System.Action<GameObject> returnCallBack)
    {
        _monoBehaviour.StartCoroutine(LoadMap(prefabPath, returnCallBack));
    }

    private IEnumerator LoadMap(string prefabPath, System.Action<GameObject> returnCallBack)
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>(prefabPath);

        UIManager.Instance.OnLoadingUI(request);

        while (!request.isDone)
        {
            yield return null;
        }

        GameObject mpaObject = request.asset as GameObject;

        returnCallBack.Invoke(mpaObject);
    }
}
