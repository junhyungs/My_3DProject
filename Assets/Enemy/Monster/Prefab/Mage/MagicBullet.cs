using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    private float m_projectileSpeed = 20.0f;
    private float m_Atk;
    private bool isFire = false;

    public void IsFire(bool isFire)
    {
        this.isFire = isFire;
        
        if(this.isFire == true)
        {
            Invoke(nameof(ReturnMagicBullet), 5.0f);
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
        PoolManager.Instance.ReturnMagicBullet(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
