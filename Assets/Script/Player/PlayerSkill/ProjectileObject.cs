using System;
using UnityEngine;

public abstract class ProjectileObject : MonoBehaviour
{
    protected Rigidbody m_projectileRigidbody;

    protected bool _isFire;

    protected float _projectileAtk;
    protected float _projectileSpeed;
    protected float _range;
    protected float _disableTime;
    private float _returnTime = 5f;

    protected virtual void Awake()
    {
        m_projectileRigidbody = GetComponent<Rigidbody>();
    }

    public abstract void IsFire(bool fire);
    public abstract void SetProjectileObjectData(float atk, float speed, float range);

    protected void InvokeReturnMethod(bool fire, string methodName)
    {
        _isFire = fire;

        if (_isFire)
        {
            Invoke(methodName, _returnTime);
        }
    }
}
