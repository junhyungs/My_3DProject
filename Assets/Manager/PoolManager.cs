using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [Header("ArrowPrefab")]
    [SerializeField] private GameObject m_arrowPrefab;
    [SerializeField] private int m_arrowCount;
    [SerializeField] private Transform m_arrowPoolPosition;
    private Queue<GameObject> ArrowPool = new Queue<GameObject>();

    [Header("FireBall")]
    [SerializeField] private GameObject m_fireBallPrefab;
    [SerializeField] private int m_fireBallCount;
    [SerializeField] private Transform m_fireBallPoolPosition;
    private Queue<GameObject> FireBallPool = new Queue<GameObject>();   

    //[Header("Monster")]
    //[SerializeField] private GameObject[] MonsterPrefab;


    private void Start()
    {
        CreateObject();
    }

    private void CreateObject()
    {
        for(int i = 0; i < m_arrowCount; i++)
        {
            GameObject arrow = Instantiate(m_arrowPrefab, m_arrowPoolPosition);
            arrow.SetActive(false);
            ArrowPool.Enqueue(arrow);
        }

        for(int i = 0; i < m_fireBallCount; i++)
        {
            GameObject fire = Instantiate(m_fireBallPrefab, m_fireBallPoolPosition);
            fire.SetActive(false);
            FireBallPool.Enqueue(fire);
        }
    }

    public GameObject GetArrow()
    {
        GameObject arrow = ArrowPool.Dequeue();
        Rigidbody arrowRigid = arrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = Vector3.zero; 
        ArrowPool.Enqueue(arrow);
        arrow.SetActive(true);
        return arrow;   
    }

    public GameObject GetFireBall()
    {
        GameObject fire = FireBallPool.Dequeue();
        FireBallPool.Enqueue(fire);
        fire.SetActive(false);
        return fire;
    }

    public void ReturnArrow(GameObject arrow)
    {
        arrow.transform.SetParent(m_arrowPoolPosition);
        arrow.SetActive(false);
    }
}
