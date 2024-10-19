using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GimikEventDic.Add(GimikEnum.SpawnMonster, SpawnEvent);
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
        int randomMonster = UnityEngine.Random.Range(1, 5);

        switch(randomMonster)
        {
            case 1:
                GameObject bat = ObjectPool.Instance.DequeueObject(ObjectName.Bat);
                BatBehaviour batComponent = bat.GetComponent<BatBehaviour>();
                batComponent.IsSpawn(true);
                RegisterMonster(bat);
                bat.transform.position = SpawnTransform.position;
                break;
            case 2:
                GameObject mage = ObjectPool.Instance.DequeueObject(ObjectName.Mage);
                MageBehaviour mageComponent = mage.GetComponent<MageBehaviour>();
                mageComponent.IsSpawn(true);
                RegisterMonster(mage);
                mage.transform.position = SpawnTransform.position;
                break;
            case 3:
                GameObject ghoul = ObjectPool.Instance.DequeueObject(ObjectName.Ghoul);
                GhoulBehaviour ghoulComponent = ghoul.GetComponent<GhoulBehaviour>();   
                ghoulComponent.IsSpawn(true);
                RegisterMonster(ghoul);
                ghoul.transform.position = SpawnTransform.position;
                break;
            case 4:
                GameObject slime = ObjectPool.Instance.DequeueObject(ObjectName.Slime);
                float sizeRandom = UnityEngine.Random.Range(0.1f, 1.5f);
                slime.transform.localScale = new Vector3(sizeRandom, sizeRandom, sizeRandom);
                SlimeBehaviour slimeComponent = slime.GetComponent<SlimeBehaviour>();
                slimeComponent.IsSpawn(true);
                RegisterMonster(slime);
                slime.transform.position = SpawnTransform.position;
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
    #endregion
}
