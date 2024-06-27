using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    protected Rigidbody m_projectileRigidbody;
    protected bool isFire;
    protected int m_atk;
    protected float m_speed;
    protected float m_range;
    protected float m_disableTime;

    protected virtual void Awake()
    {
        m_projectileRigidbody = GetComponent<Rigidbody>();
    }

    public virtual void IsFire(bool fire)
    {
        isFire = fire;
    }

    public void SetProjectileObjectData(int atk, float speed, float range)
    {
        m_atk = atk;
        m_speed = speed;
        m_range = range;
    }
}
