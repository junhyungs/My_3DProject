using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObject : MonoBehaviour
{
    private GameObject m_player;
    private Transform m_playerTransform;
    
    void Start()
    {
        Cursor.visible = false;
        m_player = GameObject.Find("player").gameObject;
        m_playerTransform = m_player.transform;
    }

    
    void Update()
    {
        MousePointObject();
        MouseObjectRotation();
    }

    private void MousePointObject()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );

        if(Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity))
        {
            Vector3 objectPosition = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);

            transform.position = objectPosition;
        }
    }

    private void MouseObjectRotation()
    {
        Vector3 dirPlayer = (m_playerTransform.position - transform.position).normalized;

        dirPlayer.y = 0f;

        float distance = Vector3.Distance(m_playerTransform.position, transform.position);

        if(distance > 0.1f)
        {

            Quaternion targetRot = Quaternion.LookRotation(-dirPlayer);

            transform.rotation = Quaternion.Euler(90.0f, targetRot.eulerAngles.y, 0f);      
        }
    }
}
