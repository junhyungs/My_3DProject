using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    
    private float m_defaultAtk;
    private float m_chargeAtk;
    private float m_currentAtk;
    private Vector3 m_defaultRange;
    private Vector3 m_chargeRange;
    private LayerMask m_monsterLayer;

    private void Awake()
    {
        EventManager.Instance.AddEvent_SetWeaponDataEvent(true, SetWeaponData);
    }

    private void Start()
    {
        EventManager.Instance.AddEvent_ActiveTriggerColliderEvent(true, OnCollider);
        EventManager.Instance.AddEvent_DeActiveTriggerColliderEvent(true, OffCollider);
        EventManager.Instance.AddEvent_SetWeaponRangeEvent(true, SetWeaponRange);
        EventManager.Instance.AddEvent_OverlapBoxEvent(true, HitOverlapBox);
    }

    public void SetWeaponData(float defaultAtk, float chargeAtk, Vector3 defaultRange, Vector3 chargeRange)
    {
        Debug.Log("SetWeaponData");
        m_defaultAtk = defaultAtk;
        m_chargeAtk = chargeAtk;
        m_defaultRange = defaultRange;
        m_chargeRange = chargeRange;
    }

    public void OnCollider()
    {
        Debug.Log("OnCollider");
    }

    public void OffCollider()
    {
        Debug.Log("OffCollider");
    }

  
    public void SetWeaponRange(bool isCharge)
    {
        if (isCharge)
        {
           
            //transform.localScale = m_chargeRange;
            //m_currentAtk = m_chargeAtk;
        }
        else
        {
           
            //transform.localScale = m_defaultRange;
            //m_currentAtk = m_defaultAtk;
        }
    }

    public void HitOverlapBox(bool isCharge)
    {
        Vector3 playerforward = transform.forward;

        Vector3 boxPosition = transform.position + playerforward;

        Vector3 boxSize = new Vector3(2.5f, 1f, 1.4f);

        Collider[] boxOverlap = Physics.OverlapBox(boxPosition, boxSize / 2 , transform.rotation, LayerMask.GetMask("Monster"));

        foreach(var box in boxOverlap)
        {
            OnHit(box);
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

        Vector3 boxSize = new Vector3(2.5f, 1f, 1.4f);
        // 플레이어의 정면 방향에서 오버랩 박스를 기즈모로 그립니다.
        DrawGizmoBox(boxPosition, boxSize, transform.rotation);
    }

    private void OnHit(Collider other)
    {
        IDamged damged = other.gameObject.GetComponent<IDamged>();

        float atk = 1f;

        damged.TakeDamage(atk);
    }

    private void OnTriggerEnter(Collider other)
    {
     
    }

}
