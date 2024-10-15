using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    
}

public class MapManager : MonoBehaviour
{
    private Dictionary<Map, GameObject> _mapDictionary;
    private MapFactory _factory;
    private GameObject _currentMap;

    private void Awake()
    {
        _factory = new MapFactory(this);
    }

    private void Start()
    {
       
    }

    //private IEnumerator LoadMapData()
    //{

    //}

    private GameObject GetMap(Map mapName)
    {
        if(_mapDictionary.TryGetValue(mapName, out GameObject map))
        {
            return map;
        }

        //뭔가 새로 생성해서 바인딩하는 로직.
        return null;
    }

    public void ChangeMap(Map mapName)
    {
        if(_currentMap != null)
        {
            _currentMap.SetActive(false);
        }
        
        switch (mapName)
        {
            case Map.MainStage:
                StartCoroutine(LoadMap(mapName));
                break;
            case Map.BossStage:
                StartCoroutine(LoadMap(mapName));
                break;
            case Map.GimikStage:
                StartCoroutine(LoadMap(mapName));
                break;
        }
    }

    private IEnumerator LoadMap(Map mapName)
    {
        yield return null;
    }
}


public class CreateObject
{
  
}