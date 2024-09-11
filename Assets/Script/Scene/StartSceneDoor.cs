using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneDoor : MonoBehaviour
{
    [Header("moveSpeed")]
    [SerializeField] private float m_moveSpeed;

    Vector3 m_startPosition;

    float m_maxDistance = 5.0f;

    float m_minDistance = -5.0f;

    bool isMovingUp;

    void Start()
    {
        m_startPosition = transform.position;

        int randomStartMovePos = Random.Range(0, 2);

        if (randomStartMovePos == 0)
            isMovingUp = true;
        else
            isMovingUp = false;
    }

    
    void Update()
    {
        if (isMovingUp)
        {
            Vector3 movePos = Vector3.up * m_moveSpeed * Time.deltaTime;

            transform.position += movePos;

            if(transform.position.y >= m_startPosition.y + m_maxDistance)
            {
                isMovingUp = false;
            }
        }
        else
        {
            Vector3 movePos = Vector3.down * m_moveSpeed * Time.deltaTime;

            transform.position += movePos;

            if(transform.position.y <= m_startPosition.y + m_minDistance)
            {
                isMovingUp = true;
            }
        }
    }
}

