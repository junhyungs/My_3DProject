using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GimikManager : Singleton<GimikManager>
{
    private Dictionary<GimikEnum, Action<GameObject>> GimikEventDic = new Dictionary<GimikEnum, Action<GameObject>>();
    private HashSet<GameObject> SpawnGimikHashSet = new HashSet<GameObject>();
    private bool isSpawnGimik;

    public Dictionary<GimikEnum, Action<GameObject>> Gimik => GimikEventDic;
    public bool OnSpawnGimik => isSpawnGimik;
    
    private void Awake()
    {
        GimikEventDic.Add(GimikEnum.OpenDoor, OpenDoor);
        
    }

    #region MoveDoor
    private void OpenDoor(GameObject eventObject)
    {
        Vector3 movePos = Vector3.down * 5.0f;

        StartCoroutine(MoveDoor(eventObject, movePos));
    }

    private IEnumerator MoveDoor(GameObject eventObject, Vector3 MovePos)
    {
        float moveTime = 4.0f;

        Vector3 startPos = eventObject.transform.position;

        Vector3 endPos = startPos + MovePos;

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            eventObject.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        eventObject.transform.position = endPos;
    }
    #endregion

    #region Monster Spawn

    public void MonsterSpawnEvent(MapData data, GameObject spawnPositionObject)
    {
        Transform spawnTransform = spawnPositionObject.transform;

        StartCoroutine(SpawnCoroutine(data, spawnTransform, data.EventCount));
    }

    private IEnumerator SpawnCoroutine(MapData data, Transform spawnTransform, int eventCount)
    {
        List<ObjectName> spawnList = ParseSpawnList(data.SpawnMonsterList);

        for (int i = 0; i < eventCount; i++)
        {
            Spawn(data, spawnTransform, spawnList);

            yield return new WaitWhile(() =>
            {
                return isSpawnGimik;
            });
        }
    }

    private void Spawn(MapData data, Transform spawnTransform, List<ObjectName> spawnList)
    {
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
           

            NavMeshAgent agent = spawnMonster.GetComponent<NavMeshAgent>();

            if (!agent.enabled)
            {
                agent.enabled = true;
            }

            spawnMonster.transform.position = spawnTransform.position;
            spawnMonster.transform.rotation = Quaternion.identity;

            spawnMonster.SetActive(true);
            RegisterMonster(spawnMonster);
        }

        isSpawnGimik = true;
    }

    private void RegisterMonster(GameObject monster)
    {
        SpawnGimikHashSet.Add(monster);
    }

    public void UnRegisterMonster(GameObject monster)
    {
        if (SpawnGimikHashSet.Contains(monster))
        {
            SpawnGimikHashSet.Remove(monster);

            if (SpawnGimikHashSet.Count == 0)
            {
                isSpawnGimik = false;
            }
        }
    }

    private List<ObjectName> ParseSpawnList(List<string> spawnList)
    {
        List<ObjectName> objectList = new List<ObjectName>();

        foreach(var spawnMonster in  spawnList)
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
    #endregion
}
