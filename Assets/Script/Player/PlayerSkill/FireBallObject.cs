using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SearchService;

public class FireBallObject : ProjectileObject
{
    private int m_piercingPower = 5;
    private bool isBurning = true;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;

        if(isFire == true)
        {
            Invoke(nameof(ReturnFireBall), 5.0f);
        }
    }

    private void FixedUpdate()
    {
        if (isFire)
        {
            Vector3 moveForce = transform.forward * m_speed;

            m_projectileRigidbody.AddForce(moveForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if(hit != null)
            {
                hit.TakeDamage(m_atk);

                m_piercingPower--;
            }
        }

        IBurningObject burning = other.gameObject.GetComponent<IBurningObject>();

        if(burning != null)
        {
            burning.OnBurning(isBurning);
        }

        if(m_piercingPower <= 0 || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ReturnFireBall();
        }

    }

    private void ReturnFireBall()
    {
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerFireBall);
    }

    
}
