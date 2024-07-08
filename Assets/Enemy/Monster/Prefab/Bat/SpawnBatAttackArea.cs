using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBatAttackArea : MonoBehaviour
{
    private int m_batAttackPower;
    private bool hasTrigger;

    public void SetAttackPower(int power)
    {
        m_batAttackPower = power;
        hasTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && hasTrigger)
        {
            hasTrigger = false;

            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if(hit != null)
            {
                hit.TakeDamage(m_batAttackPower);
            }
        }
    }
}
