using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory
{
    private MonoBehaviour _monoBehaviour;

    public MapFactory(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    public List<GameObject> CreateMapItem(List<string> pathList, Transform[] itemTransforms = null)
    {
        List<GameObject> itemList = new List<GameObject>();

        for(int i = 0; i < pathList.Count; i++)
        {
            GameObject prefab = Resources.Load<GameObject>(pathList[i]);

            GameObject item = GameObject.Instantiate(prefab);

            if (itemTransforms[i] != null)
            {
                item.transform.position = itemTransforms[i].position;
            }

            itemList.Add(item);
        }

        return itemList;    
    }
   
}
