using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : Singleton<MapManager>
{
    private Dictionary<string, MapData> _dataDictionary;
    private Dictionary<string, GameObject> _mapDictionary;

    private GameObject _currentMap;

    private void Awake()
    {
        _mapDictionary = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
       StartCoroutine(InitializeMapData());
    }

    private IEnumerator InitializeMapData()
    {
        _dataDictionary = new Dictionary<string, MapData>();

        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData("MainStage") == null;
        });

        Array enumArray = Enum.GetValues(typeof(Map));

        for(int i = 0; i < enumArray.Length; i++)
        {
            string id = enumArray.GetValue(i).ToString();

            var data = DataManager.Instance.GetData(id) as MapData;

            if(data == null)
            {
                yield break;
            }

            _dataDictionary.Add(id, data);
        }

        ChangeMap(Map.MainStage);
    }

    private MapData GetMapData(string id)
    {
        if(_dataDictionary.TryGetValue(id, out MapData data))
        {
            return data;
        }

        return null;
    }
    
    public void ChangeMap(Map mapName)
    {
        var data = GetMapData(mapName.ToString());

        InitializeMap(data);
    }

    private void InitializeMap(MapData data)
    {
        StartCoroutine(LoadPrefab(data));
    }

    private IEnumerator LoadPrefab(MapData data)
    {
        string path = data.PrefabPath;

        ResourceRequest request = Resources.LoadAsync<GameObject>(path);

        UIManager.Instance.OnLoadingUI(request);

        while (!request.isDone)
        {
            yield return null;
        }

        GameObject currentMap = Instantiate(request.asset as GameObject);

        _mapDictionary.Add(data.ID, currentMap);

        _currentMap = currentMap;

        _currentMap.SetActive(true);
    }
}
