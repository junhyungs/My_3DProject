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

    private bool isHit;

    public bool IsHit {  get { return isHit; } }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            var gimik = GimikManager.Instance.Gimik;

            if (gimik.TryGetValue(Key, out Action<GameObject> gimikEvent))
            {
                gimikEvent.Invoke(EventObject);
                isHit = true;
            }
        }    
    }

}
