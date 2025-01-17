using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    private float m_projectileSpeed = 20.0f;
    private float m_Atk;
    private bool isFire = false;
    private SphereCollider m_collider;

    private void Start()
    {
        EventManager.Instance.AddEvent_DisableMageBulletEvent(ReturnMagicBullet);
    }

    private void OnEnable()
    {
        m_collider = GetComponent<SphereCollider>();
        m_collider.enabled = false;
    }

    public void IsFire(bool isFire)
    {
        this.isFire = isFire;
        
        if(this.isFire == true)
        {
            Invoke(nameof(ReturnMagicBullet), 5.0f);
            m_collider.enabled = true;
        }
    }

    public void SetAttackPower(float attackPower)
    {
        m_Atk = attackPower;
    }

    private void FixedUpdate()
    {
        if (isFire)
        {
            Vector3 moveForce = transform.forward * m_projectileSpeed * Time.deltaTime;

            transform.position += moveForce;
        }
    }

    private void ReturnMagicBullet()
    {
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.MageBullet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamaged hit = other.gameObject.GetComponent<IDamaged>();

            if(hit != null)
            {
                hit.TakeDamage(m_Atk);
                ReturnMagicBullet();
            }
        }
    }
}
