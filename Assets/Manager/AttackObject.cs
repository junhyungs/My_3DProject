using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    private float m_defaultAtk;
    private float m_chargeAtk;
    private float m_currentAtk;
    private Vector3 m_defaultRange;
    private Vector3 m_chargeRange;
    private Vector3 m_boxposition;
    private Vector3 m_objectforward;

    private void Awake()
    {
        Debug.Log("AttackObjectAwake");
        EventManager.Instance.AddEvent_SetWeaponDataEvent(true, SetWeaponData);
    }

    private void Start()
    {
        EventManager.Instance.AddEvent_ActiveTriggerColliderEvent(true, OnCollider);
        EventManager.Instance.AddEvent_DeActiveTriggerColliderEvent(true, OffCollider);
        EventManager.Instance.AddEvent_OverlapBoxEvent(true, HitOverlapBox);
    }

    public void OnCollider()
    {
        Debug.Log("OnCollider");
    }

    public void OffCollider()
    {
        Debug.Log("OffCollider");
    }

  
    public void SetWeaponData(float defaultAtk, float chargeAtk, Vector3 defaultRange, Vector3 chargeRange)
    {
        m_defaultAtk = defaultAtk;
        m_chargeAtk = chargeAtk;
        m_defaultRange = defaultRange;
        m_chargeRange = chargeRange;
        Debug.Log(m_defaultAtk);
        Debug.Log(m_chargeAtk);
        Debug.Log(m_defaultRange);
        Debug.Log(m_chargeRange);
    }

    public void HitOverlapBox(bool isCharge)
    {
        m_objectforward = transform.forward;

        m_boxposition = transform.position + m_objectforward;

        m_currentAtk = isCharge ? m_chargeAtk : m_defaultAtk;
        
        Vector3 boxSize = isCharge ? m_chargeRange : m_defaultRange;

        Collider[] boxOverlap = Physics.OverlapBox(m_boxposition, boxSize / 2 , transform.rotation, LayerMask.GetMask("Monster"));

        foreach(var box in boxOverlap)
        {
            Hit(box);
        }
    }

    private void DrawGizmoBox(Vector3 center, Vector3 size, Quaternion rotation)
    {
        // 기즈모 색상 설정
        Gizmos.color = Color.red;

        // 회전된 오버랩 박스를 그립니다.
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = originalMatrix;
    }

    private void OnDrawGizmos()
    {
        Vector3 playerforward = transform.forward;

        Vector3 boxPosition = transform.position + playerforward;

        Vector3 boxSize = new Vector3(4f, 1f, 2.4f);
        // 플레이어의 정면 방향에서 오버랩 박스를 기즈모로 그립니다.
        DrawGizmoBox(boxPosition, boxSize, transform.rotation);
    }

    private void Hit(Collider other)
    {
        IDamged damged = other.gameObject.GetComponent<IDamged>();

        if (damged != null)
        {
            damged.TakeDamage(m_currentAtk);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
     
    }

}
