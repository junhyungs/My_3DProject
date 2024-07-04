using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekuBullet : MonoBehaviour
{
    private Rigidbody m_bulletRigid;
    private bool isFire;
    private bool isFlying;
    private int m_attackPower;
    private float m_forcePower;
    private float m_forceTime;
    

    private void Awake()
    {
        m_bulletRigid = GetComponent<Rigidbody>();  
    }

    private void OnEnable()
    {
        isFlying = false;

        m_forceTime = 0.3f;
    }

    public void SetAttackPower(int power)
    {
        m_attackPower = power;
    }

    public void IsFire(bool fire)
    {
        isFire = fire;

        if (isFire)
        {
            Invoke(nameof(ReturnBullet), 4f);
            isFlying = true;
        }
    }

    private void BulletUp()
    {
        m_forcePower = 30.0f;

        Vector3 moveDirection = transform.TransformDirection(new Vector3(0, 3, 1).normalized);

        m_bulletRigid.AddForce(moveDirection * m_forcePower, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if(isFlying && m_forceTime > 0f)
        {
            BulletUp();

            m_forceTime -= Time.fixedDeltaTime;
        }
    }

    private void ReturnBullet()
    {
        PoolManager.Instance.ReturnDekuProjectile(this.gameObject);
    }
}
