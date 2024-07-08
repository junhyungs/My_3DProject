using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GimikManager : Singleton<GimikManager>
{
    private Dictionary<int, Action<GameObject>> GimikEventDic = new Dictionary<int, Action<GameObject>>();
    private HashSet<GameObject> SpawnGimikHashSet = new HashSet<GameObject>();
    private bool isSpawnGimik;

    private enum GimikEnum
    {
        OpenDoor = 1,
        SpawnMonster = 2,
        nextSceneDoor = 3,
    }

    public Dictionary<int, Action<GameObject>> Gimik
    {
        get { return GimikEventDic;}
    }

    public bool OnSpawnGimik
    {
        get { return isSpawnGimik; }
    }

    private void Awake()
    {
        GimikEventDic.Add((int)GimikEnum.OpenDoor, OpenDoor);
        GimikEventDic.Add((int)GimikEnum.SpawnMonster, SpawnEvent);
    }

    //DoorEvent
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

    //SpawnEvent
    private void SpawnEvent(GameObject spawnPositionObject)
    {
        int spawnCount = 1;

        isSpawnGimik = true;

        StartCoroutine(SpawnMonster(spawnPositionObject.transform, spawnCount));
    }

    private IEnumerator SpawnMonster(Transform spawnPosition, int spawnCount)
    {
        for(int i = 0; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(1.0f);

            Spawn(spawnPosition);
        }
    }

    private void Spawn(Transform SpawnTransform)
    {
        int randomMonster = UnityEngine.Random.Range(1, 4);

        switch(randomMonster)
        {
            case 1:
                GameObject bat = PoolManager.Instance.GetBatMonster();
                SpawnBat batComponent = bat.GetComponent<SpawnBat>();
                batComponent.IsSpawn(true);
                RegisterMonster(bat);
                bat.transform.position = SpawnTransform.position;
                break;
            case 2:
                GameObject mage = PoolManager.Instance.GetMageMonster();
                Mage mageComponent = mage.GetComponent<Mage>();
                mageComponent.IsSpawn(true);
                RegisterMonster(mage);
                mage.transform.position = SpawnTransform.position;
                break;
            case 3:
                GameObject ghoul = PoolManager.Instance.GetGhoulMonster();
                Ghoul ghoulComponent = ghoul.GetComponent<Ghoul>();
                ghoulComponent.IsSpawn(true);
                RegisterMonster(ghoul);
                ghoul.transform.position = SpawnTransform.position;
                break;
        }
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

            if(SpawnGimikHashSet.Count == 0)
            {
                isSpawnGimik = false;
            }
        }
    }

    //nextScene



}
