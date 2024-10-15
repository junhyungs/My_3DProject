using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour
{
    private Dictionary<string, MapData> _dataDictionary;
    private HashSet<Map> _containsMap;
    private MapFactory _factory;

    private GameObject _currentMap;

    private void Awake()
    {
        _containsMap = new HashSet<Map>();

        _factory = new MapFactory(this);
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

        ChangeMap(Map.MainStage);
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
        _factory.LoadMapPrefab(data.PrefabPath, InitializeMap);
    }

    private void InitializeMap(GameObject map)
    {
        _currentMap = Instantiate(map);

        _currentMap.SetActive(true);

        //나중에 스테이지 컴포넌트 가져와서 데이터로 초기화작업.
    }

    private void LoadMap(MapData data)
    {
        _currentMap.SetActive(false);

        SaveCurrentMapData();

        Destroy(_currentMap);

        _factory.LoadMapPrefab(data.PrefabPath, LoadMap);
    }

    private void LoadMap(GameObject map)
    {
        _currentMap = Instantiate(map);

        _currentMap.SetActive(true);

        //이전 데이터를 가져와서 초기화
    }

    private void SaveCurrentMapData()
    {
        //현재 맵 데이터 저장.
    }
}
