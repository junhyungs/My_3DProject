using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class Stage : MonoBehaviour
{
    [Header("MonsterTransforms")]
    [SerializeField] private Transform[] _monsterTransforms;

    private List<GameObject> _spawnMonsterList;
    private HashSet<PlayableDirector> _timeLineSet;

    private NavMeshSurface _navMesh;
    private MapData _data;

    private void Awake()
    {
        _timeLineSet = new HashSet<PlayableDirector>();

        _spawnMonsterList = new List<GameObject>();

        _navMesh = transform.GetComponentInChildren<NavMeshSurface>();  
    }


    private void OnDisable()
    {
        foreach(var spawnMonster in _spawnMonsterList)
        {
            spawnMonster.SetActive(false);
        }

        _spawnMonsterList.Clear();
    }

    public void InitializeStage(MapData data)
    {
        _data = data;

        foreach(var spawnMonster in _data.SpawnMonsterList)
        {
            if (!string.IsNullOrWhiteSpace(spawnMonster))
            {
                var monsterType = ParseEnum(spawnMonster);

                ObjectPool.Instance.CreatePool(monsterType);
            }
        }
    }

    public void StartStage()
    {
        if(_monsterTransforms != null)
        {
            StartCoroutine(SpawnMonster(_monsterTransforms));
        }
    }

    private IEnumerator SpawnMonster(Transform[] monsterTransforms)
    {
        yield return new WaitForSeconds(10f);

        for(int i = 0; i < monsterTransforms.Length; i++)
        {
            var spawnMonster = RandomMonster();

            GameObject monster = ObjectPool.Instance.DequeueObject(spawnMonster);

            monster.transform.position = monsterTransforms[i].position;

            _spawnMonsterList.Add(monster);
        }
    }

    private ObjectName RandomMonster()
    {
        int random = UnityEngine.Random.Range(0, _data.SpawnMonsterList.Count);

        var monsterType = _data.SpawnMonsterList[random];

        return ParseEnum(monsterType);
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
