using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BombObject : ProjectileObject
{
    private int m_maxBounce = 2;
    private int m_currentBounce;
    private float m_forceTime = 0.3f;
    private float m_forcePower;
    private bool isFlying;

    protected override void Awake()
    {
        base.Awake();
        m_projectileRigidbody.useGravity = false;
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;

        if(isFire)
        {
            Invoke(nameof(ReturnBomb), 4f);
            isFlying = true;
        }
    }

    private void BombUp()
    {
        Vector3 moveDirection = transform.TransformDirection(new Vector3(0, 2, 1).normalized);

        m_projectileRigidbody.useGravity = true;

        m_projectileRigidbody.AddForce(moveDirection * (m_speed - 170f));
    }

    private void FixedUpdate()
    {
        if (isFlying && m_forceTime > 0f)
        {
            BombUp();

            m_forceTime -= Time.deltaTime;
        }
    }

    private void ReturnBomb()
    {
        PoolManager.Instance.ReturnBombObject(this.gameObject);
    }
}
