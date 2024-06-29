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

    private void Start()
    {
        WeaponManager.Instance.AddData(true, SetWeaponData);
        WeaponManager.Instance.AddOnColliderEvent(true, OnCollider);
        WeaponManager.Instance.AddOffColliderEvent(true, OffCollider);
        WeaponManager.Instance.AddWeaponRangeEvent(true, SetWeaponRange);
        m_attackCollider = GetComponent<MeshCollider>();
        m_attackCollider.enabled = false;
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
        m_attackCollider.enabled = true;
    }

    public void OffCollider()
    {
        m_attackCollider.enabled = false;
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
            transform.localScale = m_defaultRange;
            m_currentAtk = m_defaultAtk;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
