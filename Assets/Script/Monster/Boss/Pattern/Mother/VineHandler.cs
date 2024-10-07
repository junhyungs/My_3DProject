using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineHandler : MonoBehaviour
{
    protected HashSet<int> _overlapHashSet;
    protected ForestMotherAnimationEvent _aniEvent;
    protected int _index;
    protected float _damage;

    public virtual void Initialize(ForestMotherAnimationEvent aniEvent, int index,
        float damage, HashSet<int> overlapHashSet)
    {
        _aniEvent = aniEvent;
        _index = index;
        _damage = damage;
        _overlapHashSet = overlapHashSet;
    }

    protected void IsColliding(Collider other, int index)
    {
        if (_overlapHashSet.Contains(index))
        {
            return;
        }

        _overlapHashSet.Add(index);

        IDamged hit = other.gameObject.GetComponent<IDamged>();

        if (hit != null)
        {
            hit.TakeDamage(_damage);
        }
    }
}