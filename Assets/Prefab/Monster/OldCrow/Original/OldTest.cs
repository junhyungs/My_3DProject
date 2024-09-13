using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OldTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 position = transform.forward;

        Gizmos.DrawWireSphere(transform.position, 1f);

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.forward, new Vector3(1, 1, 1));
    }
}
