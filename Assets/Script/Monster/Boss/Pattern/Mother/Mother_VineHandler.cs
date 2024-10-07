using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_VineHandler : VineHandler
{
    public override void Initialize(ForestMotherAnimationEvent aniEvent, int index, 
        float damage, HashSet<int> overlapHashSet)
    {
        base.Initialize(aniEvent, index, damage, overlapHashSet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IsColliding(other, _index);
        }
    }
}

