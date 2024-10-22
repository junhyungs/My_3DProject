using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulArrow : MonoBehaviour
{
    private float m_projectileSpeed = 10.0f;
    private float m_attackPower;
    private bool isFire;
    private SphereCollider m_collider;

    private void Awake()
    {
        EventManager.Instance.AddEvent_DisableGhoulArrowEvent(ReturnArrow);
    }

    private void OnEnable()
    {
        m_collider = GetComponent<SphereCollider>();
        m_collider.enabled = false;
    }

    public void IsFire(bool isFire)
    {
        this.isFire = isFire;

        if (this.isFire)
        {
            Invoke(nameof(ReturnArrow), 3.0f);
            m_collider.enabled = true;
        }
    }

    public void SetAttackPower(float attackPower)
    {
        m_attackPower = attackPower;
    }

    private void Update()
    {
        if (isFire)
        {
            Vector3 movePos = transform.forward * m_projectileSpeed * Time.deltaTime;

            transform.position += movePos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>(); 

            if (hit != null)
            {
                hit.TakeDamage(m_attackPower);
                ReturnArrow();
            }
        }
    }

    private void ReturnArrow()
    {
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.GhoulArrow);
    }
}
