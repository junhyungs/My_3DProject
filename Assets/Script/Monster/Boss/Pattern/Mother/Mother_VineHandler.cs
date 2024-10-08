using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_VineHandler : VineHandler
{
    public override void Initialize(MotherVine motherVine, int index, float damage)
    {
        base.Initialize(motherVine, index, damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IsColliding(other, _index);
        }
    }
}

