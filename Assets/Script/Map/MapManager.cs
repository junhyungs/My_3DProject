using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour
{
    private Dictionary<string, MapData> _dataDictionary;
    private Dictionary<string, GameObject> _mapDictionary;
    private HashSet<Map> _containsMap;

    private GameObject _currentMap;

    private void Awake()
    {
        _containsMap = new HashSet<Map>();

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
                Debug.Log("현재 가져온 맵 데이터 중 null이 반환되었습니다.");
                yield break;
            }

            _dataDictionary.Add(id, data);
        }

        ChangeMap(Map.GimikStage);
    }

    private MapData GetMapData(string id)
    {
        if(_dataDictionary.TryGetValue(id, out MapData data))
        {
            return data;
        }

        Debug.Log("<MapManager> GetMapData가 실패했습니다.");
        return null;
    }
    
    public void ChangeMap(Map mapName)
    {
        var data = GetMapData(mapName.ToString());

        if (_containsMap.Contains(mapName))
        {
            LoadMap(data);

            return;
        }

        InitializeMap(data);

        _containsMap.Add(mapName);
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

        Stage stage = _currentMap.GetComponent<Stage>();

        if (stage != null)
        {
            stage.InitializeStage(data);

            stage.StartStage();
        }
    }

    private void LoadMap(MapData data)
    {
        UIManager.Instance.OnLoadingUI();

        SaveCurrentMapData();

        _currentMap.SetActive(false);

        GameObject nextMap = GetMap(data.ID);

        _currentMap = nextMap;

        _currentMap.SetActive(true);
    }

    //이미 헤쉬셋으로 있는지 확인했기 때문에 바로 리턴.
    private GameObject GetMap(string id)
    {
        return _mapDictionary[id];
    }

    private void SaveCurrentMapData()
    {
        //현재 맵 데이터 저장.
    }
}
