using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pool
{
    public Queue<GameObject> _queue;
    public PrefabPath _objectPath;
    public Transform _transform;
    public int _count;

    public Pool(Transform transform, PrefabPath objectPath)
    {
        _queue = new Queue<GameObject>();
        _count = 0;
        _transform = transform;
        _objectPath = objectPath;
    }
}

public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<ObjectName, Pool> _objectPool = new Dictionary<ObjectName, Pool>();
    private Dictionary<ObjectName, GameObject> _objectDictionary = new Dictionary<ObjectName, GameObject>();
    private HashSet<ObjectName> _creatingPool = new HashSet<ObjectName>();
    
    //코루틴을 실행할 수 없을 때
    public void CreateObject(string id)
    {
        StartCoroutine(CreatePool(id));
    }

    //풀 생성 코루틴
    public IEnumerator CreatePool(string id, int count = 20)
    {
        if (_creatingPool.Contains(ParseEnum(id)))
        {
            yield break;
        }

        _creatingPool.Add(ParseEnum(id));   

        yield return new WaitWhile(() =>
        {
            Debug.Log("프리팹 경로를 가져오지 못했습니다.");
            return DataManager.Instance.GetPath(id) == null;
        });

        var pathData = DataManager.Instance.GetPath(id) as PrefabPath;

        ObjectName gameObjectType = ParseEnum(id);

        if (!_objectPool.ContainsKey(gameObjectType))
        {
            GameObject pool = new GameObject();
            pool.transform.SetParent(this.transform);
            pool.name = id + "Pool";
            _objectPool.Add(gameObjectType, new Pool(pool.transform, pathData));
        }

        CreatePrefab(pathData, count, gameObjectType);

        _creatingPool.Remove(gameObjectType);
    }

    private void CreatePrefab(PrefabPath pathData, int count, ObjectName name)
    {
        GameObject prefab = Resources.Load<GameObject>(pathData.Path);

        if (!_objectDictionary.ContainsKey(name))
        {
            _objectDictionary.Add(name, prefab);
        }

        for(int i = 0; i < count; i++)
        {
            GameObject item = Instantiate(prefab, _objectPool[name]._transform);

            item.name = prefab.name;
            item.SetActive(false);
            _objectPool[name]._queue.Enqueue(item);
            _objectPool[name]._count++;
        }
    }

    private ObjectName ParseEnum(string id)
    {
        ObjectName result;

        if (!Enum.TryParse(id, true, out result))
        {
            result = ObjectName.Null;
        }

        return result;
    }

    public void EnqueueObject(GameObject prefab, ObjectName name)
    {
        if (!_objectPool.ContainsKey(name))
        {
            Debug.Log("풀이 없습니다.");
            StartCoroutine(CreatePool(name.ToString()));
        }

        prefab.transform.SetParent(_objectPool[name]._transform);
        prefab.SetActive(false);
        _objectPool[name]._queue.Enqueue(prefab);
    }

    public GameObject DequeueObject(ObjectName name)
    {
        if (!_objectPool.ContainsKey(name))
        {
            Debug.Log("풀이 없습니다.");
            StartCoroutine(CreatePool(name.ToString()));
        }

        if (_objectPool[name]._queue.Count == 0)
        {
            GameObject previousObject = _objectDictionary[name];

            GameObject newObject = Instantiate(previousObject, _objectPool[name]._transform);

            newObject.SetActive(true);

            return newObject;
        }

        GameObject item = _objectPool[name]._queue.Dequeue();

        return item;
    }
}


