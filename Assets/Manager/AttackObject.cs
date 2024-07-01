using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    
    private MeshCollider m_attackCollider;
    private float m_defaultAtk;
    private float m_chargeAtk;
    private float m_currentAtk;
    private Vector3 m_defaultRange;
    private Vector3 m_chargeRange;
    private LayerMask m_monsterLayer;

    private void Awake()
    {
        WeaponManager.Instance.AddData(true, SetWeaponData);
    }

    private void Start()
    {
        WeaponManager.Instance.AddOnColliderEvent(true, OnCollider);
        WeaponManager.Instance.AddOffColliderEvent(true, OffCollider);
        WeaponManager.Instance.AddWeaponRangeEvent(true, SetWeaponRange);
        m_attackCollider = GetComponent<MeshCollider>();
        m_attackCollider.enabled = false;
        m_monsterLayer = LayerMask.NameToLayer("Monster");
    }

    public void SetWeaponData(float defaultAtk, float chargeAtk, Vector3 defaultRange, Vector3 chargeRange)
    {
        m_defaultAtk = defaultAtk;
        m_chargeAtk = chargeAtk;
        m_defaultRange = defaultRange;
        m_chargeRange = chargeRange;
        transform.localScale = m_defaultRange;
    }

    public void OnCollider()
    {
        //m_attackCollider.enabled = true;
    }

    public void OffCollider()
    {
        //m_attackCollider.enabled = false;
    }

    public void SetWeaponRange(bool isCharge)
    {
        if (isCharge)
        {
            transform.localScale = m_chargeRange;
            m_currentAtk = m_chargeAtk;
        }
        else
        {
            HitOverlapSphere(isCharge);
            transform.localScale = m_defaultRange;
            m_currentAtk = m_defaultAtk;
        }

    }

    private void HitOverlapSphere(bool isCharge)
    {
        Debug.Log("½ÇÇàµÊ");
        float sphereRadisu = isCharge ? 1.62f : 2.5f;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadisu, m_monsterLayer);

        Vector3 forward = transform.forward;

        foreach(var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.gameObject.name);
            Vector3 toCollider = (hitCollider.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(forward, toCollider);

            if(dot > 0)
            {
                IDamged hit = hitCollider.gameObject.GetComponent<IDamged>();

                if(hit != null)
                {
                    hit.TakeDamage(m_defaultAtk);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);   
        //if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        //{
        //    IDamged hit = other.gameObject.GetComponent<IDamged>();

        //    if (hit != null)
        //    {
        //        GameObject HitParticle = PoolManager.Instance.GetHitParticle();
        //        ParticleSystem HitParticleSystem = HitParticle.GetComponent<ParticleSystem>();
        //        HitParticle.transform.position = other.transform.position;
        //        HitParticle.SetActive(true);
        //        HitParticleSystem.Play();   
        //        SkillManager.Instance.AddSkillCount();
        //        hit.TakeDamage(m_currentAtk);
        //    }
        //}

        if(other.gameObject.layer == LayerMask.NameToLayer("GimikObject"))
        {
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }

}
