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

    private MapData _data;

    private void Awake()
    {
        _timeLineSet = new HashSet<PlayableDirector>();

        _spawnMonsterList = new List<GameObject>();
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

        CreateMonster(_data);
    }

    private void CreateMonster(MapData data)
    {
        foreach (var spawnMonster in data.SpawnMonsterList)
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
        yield return new WaitForEndOfFrame();

        for(int i = 0; i < monsterTransforms.Length; i++)
        {
            var spawnMonster = RandomMonster();

            GameObject monster = ObjectPool.Instance.DequeueMonster(spawnMonster);

            monster.transform.position = monsterTransforms[i].position;

            monster.SetActive(true);

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
