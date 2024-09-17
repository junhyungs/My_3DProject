using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pool
{
    public Queue<GameObject> _queue;
    public Transform _transform;
    public int _count;

    public Pool(Transform transform)
    {
        _queue = new Queue<GameObject>();
        _count = 0;
        _transform = transform;
    }
}

public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<ObjectName, Pool> _objectPool = new Dictionary<ObjectName, Pool>();
    private Dictionary<ObjectName, GameObject> _objectDictionary = new Dictionary<ObjectName, GameObject>();
    private Dictionary<ObjectName, PrefabPath> _pathDataDictionary = new Dictionary<ObjectName, PrefabPath>();

    private bool _poolDataReady = false;

    private event Action _poolDelayAction;
    private event Action _dequeueDelayAction;

    private void Awake()
    {
        StartCoroutine(LoadPathData()); 
    }
    
    private IEnumerator LoadPathData()
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("���� ��� �����͸� �������� ���߽��ϴ�.");
            return DataManager.Instance.GetPath(ObjectName.Player.ToString()) == null;
        });

        Array valueArray = Enum.GetValues(typeof(ObjectName));

        int enumLength = valueArray.Length - 1;

        for(int i = 0; i < enumLength; i++)
        {
            ObjectName name = (ObjectName)valueArray.GetValue(i);

            var pathData = DataManager.Instance.GetPath(name.ToString()) as PrefabPath;
            
            _pathDataDictionary.Add(name, pathData);
        }

        _poolDataReady = true;
        _poolDelayAction?.Invoke();
        _poolDelayAction = null;
    }

    #region CreatePool
    public void CreatePool(ObjectName name, int count = 20)
    {
        if(name is ObjectName.PlayerHook)
        {
            Debug.Log("��");
        }

        if (!_poolDataReady)
        {
            _poolDelayAction += () => CreatePool(name, count);

            return;
        }

        if (!_objectPool.ContainsKey(name))
        {
            GameObject pool = new GameObject();
            pool.transform.SetParent(this.transform);
            pool.name = name.ToString() + "Pool";
            _objectPool.Add(name, new Pool(pool.transform));
        }

        var pathData = _pathDataDictionary[name];

        CreatePrefab(pathData, count, name);

    }

    private void CreatePrefab(PrefabPath pathData, int count, ObjectName name)
    {
        GameObject prefab = Resources.Load<GameObject>(pathData.Path);

        if (!_objectDictionary.ContainsKey(name))
        {
            _objectDictionary.Add(name, prefab);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject item = Instantiate(prefab, _objectPool[name]._transform);

            item.name = prefab.name;
            item.SetActive(false);
            _objectPool[name]._queue.Enqueue(item);
            _objectPool[name]._count++;
        }
    }
    #endregion

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
            Debug.Log("Ǯ�� �����ϴ�.");
            CreatePool(name);
        }

        prefab.transform.SetParent(_objectPool[name]._transform);
        prefab.SetActive(false);
        _objectPool[name]._queue.Enqueue(prefab);
    }

    public GameObject DequeueObject(ObjectName name)
    {
        if (!_poolDataReady)
        {
            //TODO CSV���� ���� �ϳ� ���� �ű⿡�� �н��� �־ ���� ������ �ε����̸� �װ� ���������� �ؾ߰���.
            Debug.Log("�����Ͱ� �ε���� �ʾҽ��ϴ�.");
            return null;
        }

        if (!_objectPool.ContainsKey(name) && _poolDataReady)
        {
            Debug.Log("�����ʹ� �ε�Ǿ����� Ǯ�� �����ϴ�.");
            CreatePool(name);
        }

        if (_objectPool[name]._queue.Count == 0)
        {
            GameObject newObject = CreateItem(name);

            return newObject;   
        }

        GameObject item = _objectPool[name]._queue.Dequeue();

        item.SetActive(true);

        return item;
    }

    private GameObject CreateItem(ObjectName name)
    {
        GameObject previousObject = _objectDictionary[name];

        GameObject newObject = Instantiate(previousObject, _objectPool[name]._transform);

        newObject.SetActive(true);

        return newObject;
    }

    public GameObject GetPlayerSegment(Vector3 position, Quaternion rotation, ObjectName name)
    {
        if (_objectPool[name]._queue.Count > 0)
        {
            GameObject playerSegment = DequeueObject(name);
            playerSegment.transform.position = position;
            playerSegment.transform.rotation = rotation;
            playerSegment.SetActive(true);
            return playerSegment;
        }
        else
        {
            GameObject newSegment = CreateItem(name);
            newSegment.transform.position = position;
            newSegment.transform.rotation = rotation;
            return newSegment;
        }
    }


    #region CoroutinePool
    //Ǯ ���� �ڷ�ƾ
    //������Ʈ �̸� �̳��� ��Ʈ������ ��ȯ�� �־��ָ� ��
    //public IEnumerator CreatePool(string id, int count = 20)
    //{
    //    if (_creatingPool.Contains(ParseEnum(id)))
    //    {
    //        yield break;
    //    }

    //    _creatingPool.Add(ParseEnum(id));   

    //    yield return new WaitWhile(() =>
    //    {
    //        Debug.Log("������ ��θ� �������� ���߽��ϴ�.");
    //        return DataManager.Instance.GetPath(id) == null;
    //    });

    //    var pathData = DataManager.Instance.GetPath(id) as PrefabPath;

    //    ObjectName gameObjectType = ParseEnum(id);

    //    if (!_objectPool.ContainsKey(gameObjectType))
    //    {
    //        GameObject pool = new GameObject();
    //        pool.transform.SetParent(this.transform);
    //        pool.name = id + "Pool";
    //        _objectPool.Add(gameObjectType, new Pool(pool.transform, pathData));
    //    }

    //    CreatePrefab(pathData, count, gameObjectType);

    //    _creatingPool.Remove(gameObjectType);
    //}
    #endregion
}


