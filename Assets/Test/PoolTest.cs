using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    private bool isFire;
    private void Awake()
    {
        ObjectPool.Instance.CreatePool(ObjectName.Soul);
        ObjectPool.Instance.CreatePool(ObjectName.GhoulArrow);
    }

    void Start()
    {
        StartCoroutine(TT());
    }

    private IEnumerator TT()
    {
        while (true)
        {
            GameObject soul = ObjectPool.Instance.DequeueObject(ObjectName.Soul);

            if(soul != null)
            {
                soul.transform.position = gameObject.transform.position + new Vector3(1, 1, 1);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
