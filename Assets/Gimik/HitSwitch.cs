using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : MonoBehaviour
{
    [Header("Key")]
    [SerializeField] private int Key;

    [Header("GameObject")]
    [SerializeField] private GameObject EventObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            var gimik = GimikManager.Instance.Gimik;

            if (gimik.TryGetValue(Key, out Action<GameObject> gimikEvent))
            {
                gimikEvent.Invoke(EventObject);
            }
        }    
    }

}
