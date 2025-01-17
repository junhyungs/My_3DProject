using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineHandler : MonoBehaviour
{
    protected MotherVine _motherVine;
    protected int _index;
    protected float _damage;

    public virtual void Initialize(MotherVine motherVine, int index, float damage)
    {
        _motherVine = motherVine;
        _index = index;
        _damage = damage;
    }

    protected void IsColliding(Collider other, int index)
    {
        if (_motherVine._overlapHashSet.Contains(index))
        {
            return;
        }

        _motherVine._overlapHashSet.Add(index);

        IDamaged hit = other.gameObject.GetComponent<IDamaged>();

        if (hit != null)
        {
            hit.TakeDamage(_damage);
        }
    }
}