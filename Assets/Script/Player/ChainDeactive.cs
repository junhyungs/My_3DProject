using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDeactive : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("fly"))
        {
            ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerSegment);
        }
    }

}
