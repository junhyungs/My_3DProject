using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallObject : ProjectileObject
{
    private int m_piercingPower = 5;

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
