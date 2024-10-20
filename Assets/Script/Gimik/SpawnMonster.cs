using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMonster : MonoBehaviour
{
    [Header("SpwanPosition")]
    [SerializeField] private GameObject[] _spawnPositionArray;

    [Header("DoorObject")]
    [SerializeField] private GameObject[] _doorObjectArray;

    public MapData Data { get; set; }

    private SphereCollider _triggerCollider;
    private Material[] _doorMaterialArray;
    private HashSet<GameObject> _spawnMonsterSet;

    private float _time;
    private float _colorAmount;
    private int _spawnCount;
    private bool _isSpawn;

    private void Awake()
    {
        _triggerCollider = GetComponent<SphereCollider>();  
        
        _spawnMonsterSet = new HashSet<GameObject>();
    }

    private void OnEnable()
    {
        _triggerCollider.enabled = true;

        _isSpawn = false;
    }

    private void Start()
    {
        _doorMaterialArray = new Material[_doorObjectArray.Length];

        for(int i = 0; i < _doorMaterialArray.Length; i++)
        {
            MeshRenderer renderer = _doorObjectArray[i].GetComponent<MeshRenderer>();

            if(renderer != null)
            {
                _doorMaterialArray[i] = renderer.material;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartSpawnEvent();
        }
    }

    public void ResetSpawnGimik()
    {
        _spawnMonsterSet.Clear();
    }

    private void StartSpawnEvent()
    {
        _triggerCollider.enabled = false;

        StartCoroutine(DoorCoroutine());
    }

    private IEnumerator DoorCoroutine()
    {
        yield return StartCoroutine(Open());

        //foreach(var spawnPosition in _spawnPositionList)
        //{
        //    yield return StartCoroutine(MonsterSpawnEvent(Data, spawnPosition));
        //}



        for(int i = 0; i < Data.EventCount; i++)
        {
            if(i < 3)
            {
                
            }
        }
    }

    private IEnumerator MonsterSpawnEvent(MapData data, GameObject spawnPositionObject)
    {
        if(Data == null)
        {
            Debug.Log("맵 데이터가 null입니다.");
            yield break;
        }

        Transform spawnTransform = spawnPositionObject.transform;

        List<ObjectName> spawnList = ParseSpawnList(data.SpawnMonsterList);

        for(int i = 0; i < data.EventCount; i++)
        {
            Spawn(data, spawnTransform, spawnList);

            yield return new WaitWhile(() =>
            {
                return _isSpawn;
            });
        }

        StartCoroutine(Close());
    }

    private void Spawn(MapData data, Transform spawnTransform, List<ObjectName> spawnList)
    {
        _spawnMonsterSet.Clear();

        for(int i = 0; i < data.SpawnCount; i++)
        {
            int random = UnityEngine.Random.Range(0, spawnList.Count);
            var spawnMonster = ObjectPool.Instance.DequeueMonster(spawnList[random]);

            if (spawnList[random] is ObjectName.Slime)
            {
                float sizeRandom = UnityEngine.Random.Range(0.1f, 1.5f);
                spawnMonster.transform.localScale = new Vector3(sizeRandom, sizeRandom, sizeRandom);
            }

            BehaviourMonster monsterComponent = spawnMonster.GetComponent<BehaviourMonster>();
            monsterComponent.IsSpawn(true, this);

            NavMeshAgent agent = spawnMonster.GetComponent<NavMeshAgent>();

            if(agent != null)
            {
                if (!agent.enabled)
                {
                    agent.enabled = true;
                }
            }

            spawnMonster.transform.position = spawnTransform.position;
            spawnMonster.transform.rotation = Quaternion.identity;

            RegisterMonster(spawnMonster);
            spawnMonster.SetActive(true);
        }

        _isSpawn = true;
    }

    private void RegisterMonster(GameObject monsterObject)
    {
        _spawnMonsterSet.Add(monsterObject);
    }

    public void UnRegisterMonster(GameObject monsterObject)
    {
        if (!_spawnMonsterSet.Contains(monsterObject))
        {
            return;
        }

        _spawnMonsterSet.Remove(monsterObject);

        if(_spawnMonsterSet.Count <= 0)
        {
            _isSpawn = false;
        }
    }

    private List<ObjectName> ParseSpawnList(List<string> spawnList)
    {
        List<ObjectName> objectList = new List<ObjectName>();

        foreach (var spawnMonster in spawnList)
        {
            var parseEnum = ParseEnum(spawnMonster);

            objectList.Add(parseEnum);
        }

        return objectList;
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

    private IEnumerator Close()
    {
        _time = 2.5f;

        _colorAmount = 0.006f;

        float colorValue = 0.5f;

        while(_time > 0f)
        {
            colorValue -= _colorAmount;

            for(int i = 0; i < _doorMaterialArray.Length; i++)
            {
                _doorMaterialArray[i].SetFloat("_Float", colorValue);
            }

            yield return null;

            _time -= Time.deltaTime;
        }
    }

    private IEnumerator Open()
    {
        _time = 2.5f;

        _colorAmount = 0.006f;

        float colorValue = -0.3f;

        while(_time > 0f)
        {
            colorValue += _colorAmount;

            for(int i= 0; i < _doorMaterialArray.Length; i++)
            {
                _doorMaterialArray[i].SetFloat("_Float", colorValue);
            }

            yield return null;

            _time -= Time.deltaTime;
        }
    }
}
