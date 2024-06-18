using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowObject : ProjectileObject
{
    protected override void Awake()
    {
        base.Awake();      
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;
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
        
    }

}
