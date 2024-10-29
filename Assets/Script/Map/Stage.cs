using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stage : MonoBehaviour
{
    private enum SkyBox
    {
        Default,
        Sky_Pano
    }

    [Header("SpawnTransform")]
    [SerializeField] private Transform[] _monsterTransforms;
    [SerializeField] private Item[] _items;

    protected List<BehaviourMonster> _spawnMonsters = new List<BehaviourMonster>();
    protected List<Material> _skyBoxList;
    protected Material _skyBoxMaterial;
    protected MapData _data;
    protected Transform _saveTransform;

    public virtual void SetMapData(MapData data)
    {
        _data = data;
    }

    //SkyBoX 변경 메서드
    public virtual void ChangeSkyBox()
    {
        if(_skyBoxMaterial == null)
        {
            _skyBoxMaterial = Resources.Load<Material>(_data.SkyBoxPath);
        }

        RenderSettings.skybox = _skyBoxMaterial;
    }

    protected virtual void OnDisable()
    {
        OnDisableMap();
    }

    public void OnDisableMap()
    {
        if(_spawnMonsters.Count <= 0)
        {
            return;
        }

        foreach(var monster in _spawnMonsters)
        {
            if(monster != null)
            {
                monster.OnDisableMonster();
            }
        }
    }

    public virtual void SpawnItems()
    {
        for(int i = 0; i < _items.Length; i++)
        {
            var itemType = _items[i]._itemType;

            if(InventoryManager.Instance.ItemSet.Contains(itemType))
            {
                _items[i].gameObject.SetActive(false);
            }
        }
    }

    public virtual void CreateMonsters()
    {
        foreach(var spawnMonster in _data.SpawnMonsterList)
        {
            if (!string.IsNullOrWhiteSpace(spawnMonster))
            {
                var monsterType = ParseEnum(spawnMonster);

                ObjectPool.Instance.CreatePool(monsterType);
            }
        }

        if (_monsterTransforms.Length <= 0)
        {
            return;
        }

        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        yield return new WaitForEndOfFrame();

        for(int i = 0; i < _monsterTransforms.Length; i++)
        {
            var monsterType = RandomMonster();

            GameObject monster = ObjectPool.Instance.DequeueMonster(monsterType);

            monster.transform.position = _monsterTransforms[i].position;

            NavMeshAgent agent = monster.GetComponent<NavMeshAgent>();

            if (!agent.enabled)
            {
                agent.enabled = true;
            }

            monster.SetActive(true);

            BehaviourMonster monsterComponent = monster.GetComponent<BehaviourMonster>();
            
            _spawnMonsters.Add(monsterComponent);
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
