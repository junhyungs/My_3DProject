using System.Collections;
using UnityEngine;

public class MapFactory
{
    private MonoBehaviour _monoBehaviour;

    public MapFactory(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void LoadMapPrefab(string mapPath, System.Action<GameObject> returnCallBack)
    {
        _monoBehaviour.StartCoroutine(LoadMap(mapPath, returnCallBack));
    }

    private IEnumerator LoadMap(string mapPath, System.Action<GameObject> returnCallBack)
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>(mapPath);

        UIManager.Instance.OnLoadingUI(request);

        while (!request.isDone)
        {
            yield return null;
        }

        GameObject mapPrefab = request.asset as GameObject;

        returnCallBack.Invoke(mapPrefab);
    }
}
