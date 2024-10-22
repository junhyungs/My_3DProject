using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : Singleton<MapManager>
{
    private Dictionary<string, MapData> _dataDictionary;
    private Dictionary<string, GameObject> _mapDictionary;
    private GameObject _currentMap;
    private Map _currentMapName;
    public Map CurrentMapName => _currentMapName;
    
    private void Awake()
    {
        _mapDictionary = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
       UIManager.Instance.OnInitializeImage(true);

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

        UIManager.Instance.OnInitializeImage(false);

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
    
    //¸Ê º¯°æ ½Ã È£Ãâ.
    public void ChangeMap(Map mapName)
    {
        var data = GetMapData(mapName.ToString());

        _currentMapName = mapName;

        if (_mapDictionary.ContainsKey(data.ID))
        {
            LoadMap(mapName);

            return;
        }

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

        if(_currentMap !=null)
        {
            _currentMap.SetActive(false);
        }

        GameObject currentMap = Instantiate(request.asset as GameObject);

        _mapDictionary.Add(data.ID, currentMap);

        _currentMap = currentMap;

        _currentMap.SetActive(true);

        if(_currentMap.TryGetComponent(out Stage stageComponent))
        {
            stageComponent.SetMapData(data);

            stageComponent.SpawnItems();

            stageComponent.CreateMonsters();
        }
    }

    private void LoadMap(Map mapName)
    {
        UIManager.Instance.OnLoadingUI(null);   

        if(_currentMap != null)
        {
            _currentMap.SetActive(false);
        }

        string mapDictionaryKey = mapName.ToString();

        if (_mapDictionary.TryGetValue(mapDictionaryKey, out GameObject nextMap))
        {
            _currentMap = nextMap;

            _currentMap.SetActive(true);

            if (_currentMap.TryGetComponent(out Stage stageComponent))
            {
                stageComponent.SpawnItems();

                stageComponent.CreateMonsters();
            }
            else
                Debug.Log("Stage ÄÄÆ÷³ÍÆ®°¡ ¾ø½À´Ï´Ù.");
        }
        else
            Debug.Log("¸ÊÀÌ µñ¼Å³Ê¸®¿¡ ¾ø½À´Ï´Ù.");
    }

    public void Respawn()
    {
        if(_currentMap != null )
        {
            ChangeMap(_currentMapName);
        }
    }
   
}
