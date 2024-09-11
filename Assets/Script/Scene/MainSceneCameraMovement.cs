using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MainSceneCameraMovement : MonoBehaviour
{
    [Header("MoveSpeed")]
    [SerializeField] private float m_moveSpeed;

    private List<Transform> CameraMoveList = new List<Transform>();
    private bool isMove;
    private int m_random;

    void Start()
    {
       FindMovePos();
    }

    
    void Update()
    {
        Movement();
    }

    private void FindMovePos()
    {
        GameObject movePos = transform.parent.gameObject;

        foreach(Transform pos in movePos.transform)
        {
            CameraMoveList.Add(pos);
        }
    }

    private void Movement()
    {
        if (!isMove)
        {
            m_random = Random.Range(0, CameraMoveList.Count);
            isMove = true;
        }
        
        Vector3 nextMovePos = CameraMoveList[m_random].position;

        transform.position = Vector3.MoveTowards(transform.position, nextMovePos, m_moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, nextMovePos) < 0.01f)
        {
            isMove = false;
        }
    }
}
