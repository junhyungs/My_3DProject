using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropSoul : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float m_moveSpeed;

    [Header("Soul")]
    [SerializeField] private int m_soulValue;

    private GameObject m_player;
    private Rigidbody m_soulRigid;
    private float m_Power = 400.0f;

    private void OnEnable()
    {
        m_soulRigid = GetComponent<Rigidbody>();
        m_player = GameManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        if (m_player.activeSelf)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_player.transform.position + new Vector3(0, 0.1f, 0), m_moveSpeed * Time.deltaTime);
        }
        else
            gameObject.SetActive(false);
    }

    public int GetSoulValue()
    {
        return m_soulValue;
    }

    public IEnumerator Fly()
    {
        float randomPosX = Random.Range(-0.5f, 0.5f);
        float randomPosZ = Random.Range(-0.5f, 0.5f);

        Vector3 movePos = new Vector3(randomPosX, 1.5f, randomPosZ) * m_Power;

        m_soulRigid.AddForce(movePos, ForceMode.Force);

        yield return new WaitForSeconds(0.7f);

        m_soulRigid.velocity = Vector3.zero;
        m_soulRigid.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")
            || other.gameObject.layer == LayerMask.NameToLayer("HitPlayer"))
        {
            ReturnSoul();
        }
    }

    private void ReturnSoul()
    {
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.Soul);
    }

}
