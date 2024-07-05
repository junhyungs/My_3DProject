using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowObject : ProjectileObject
{
    [Header("FireParticle")]
    [SerializeField] private GameObject m_fireParticleObject;


    private bool isBurning = false;

    protected override void Awake()
    {
        base.Awake();      
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;

        if(isFire == true)
        {
            Invoke(nameof(ReturnArrow), 5.0f);
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
                ReturnArrow();
            }
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("HitSwitch"))
        {
            ReturnArrow();
        }

        Stove isFire = other.gameObject.GetComponent<Stove>();

        if (isFire != null && isBurning == false)
        {
            bool burning = isFire.IsBurning;
            OnFireParticle(burning);
        }

        if (isBurning)
        {
            IBurningObject burning = other.gameObject.GetComponent<IBurningObject>();
            
            if(burning != null)
            {
                burning.OnBurning(true);
            }

        }
    }

    private void OnFireParticle(bool buring)
    {
        if (buring)
        {
            m_fireParticleObject.SetActive(true);
            this.isBurning = buring;
        }
    }

    private void ReturnArrow()
    {
        PoolManager.Instance.ReturnArrow(this.gameObject);
    }

}
