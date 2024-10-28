using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMonster : MonoBehaviour
{
    [Header("DoorObject")]
    [SerializeField] private GameObject[] _doorObjectArray;

    [Header("SelectDoor")]
    [SerializeField] private int _doorCount;

    [Header("EndingDoor")]
    [SerializeField] private GameObject _endingDoor;

    public MapData Data { get; set; }

    private SphereCollider _triggerCollider;
    private HashSet<GameObject> _spawnMonsterSet;
    private Dictionary<GameObject, Material> _doorMaterialDictionary;

    private float _colorAmount;
    private int _spawnFailCount;
    private bool _isSpawn;

    private void Awake()
    {
        _triggerCollider = GetComponent<SphereCollider>();  
        
        _spawnMonsterSet = new HashSet<GameObject>();
    }

    private void OnEnable()
    {
        _triggerCollider.enabled = true;

        _spawnFailCount = 0;

        _isSpawn = false;
    }

    private void OnDisable()
    {
        foreach(var monster in _spawnMonsterSet)
        {
            BehaviourMonster monsterComponent = monster.GetComponent<BehaviourMonster>();

            monsterComponent.OnDisableMonster();
        }
    }

    private void Start()
    {
        _doorMaterialDictionary = new Dictionary<GameObject, Material>();

        for(int i = 0; i < _doorObjectArray.Length; i++)
        {
            MeshRenderer renderer = _doorObjectArray[i].GetComponent<MeshRenderer>();

            if(renderer != null)
            {
                Material copyMaterial = Instantiate(renderer.material);

                renderer.material = copyMaterial;

                _doorMaterialDictionary.Add(_doorObjectArray[i], renderer.material);
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

    private Material GetDoorMaterial(GameObject selectObject)
    {
        if (_doorMaterialDictionary.ContainsKey(selectObject))
        {
            return _doorMaterialDictionary[selectObject];
        }

        Debug.Log("머트리얼이 없습니다.");
        return null;
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
        if(_doorCount > _doorObjectArray.Length)
        {
            Debug.Log("선택할 수는 전체 Door 배열 크기를 초과할 수 없습니다.");
            yield break;
        }

        GameObject[] selectDoor = new GameObject[_doorCount];

        for (int i = 0; i < Data.EventCount; i++)
        {
            _spawnMonsterSet.Clear();

            GameObject[] spawnDoorArray = SelectArray(selectDoor);

            for(int k = 0; k < spawnDoorArray.Length; k++)
            {
                StartCoroutine(Open(spawnDoorArray[k], Data));
            }

            _isSpawn = true;

            yield return new WaitWhile(() =>
            {
                if(_spawnFailCount >= spawnDoorArray.Length)
                {
                    _isSpawn = false;
                }

                return _isSpawn;
            });
        }

        _endingDoor.SetActive(true);
    }

    private IEnumerator Open(GameObject doorObject, MapData data)
    {
        float time = 2.5f;

        _colorAmount = 0.006f;

        float colorValue = -0.3f;

        Material doorMaterial = GetDoorMaterial(doorObject);

        while (time > 0f)
        {
            colorValue += _colorAmount;

            doorMaterial.SetFloat("_Float", colorValue);

            yield return null;

            time -= Time.deltaTime;
        }

        if(data == null)
        {
            Debug.Log("스폰 데이터가 null 입니다.");

            _spawnFailCount++;

            yield break;
        }

        Transform spawnTransform = doorObject.transform.GetChild(0).transform;

        List<ObjectName> monsterList = ParseSpawnList(data.SpawnMonsterList);

        Spawn(data, spawnTransform, monsterList, doorObject);
    }

    private GameObject[] SelectArray(GameObject[] selectDoor)
    {
        System.Random random = new System.Random();

        for(int i = _doorObjectArray.Length - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);

            GameObject tempObject = _doorObjectArray[i];

            _doorObjectArray[i] = _doorObjectArray[randomIndex];

            _doorObjectArray[randomIndex] = tempObject;
        }

        for(int i = 0; i < selectDoor.Length; i++)
        {
            selectDoor[i] = _doorObjectArray[i];
        }

        return selectDoor;
    }

    private void Spawn(MapData data, Transform spawnTransform, List<ObjectName> spawnList, GameObject doorObject)
    {
        for(int i = 0; i < data.SpawnCount; i++)
        {
            int random = UnityEngine.Random.Range(0, spawnList.Count);
            var spawnMonster = ObjectPool.Instance.DequeueMonster(spawnList[random]);

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

        StartCoroutine(Close(doorObject));
    }

    private IEnumerator Close(GameObject doorObject)
    {
        float time = 2.5f;

        _colorAmount = 0.006f;

        float colorValue = 0.5f;

        Material doorMaterial = GetDoorMaterial(doorObject);

        while (time > 0f)
        {
            colorValue -= _colorAmount;

            doorMaterial.SetFloat("_Float", colorValue);

            yield return null;

            time -= Time.deltaTime;
        }
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
}
