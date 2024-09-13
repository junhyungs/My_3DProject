using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ForestMother : MonoBehaviour
{
    [SerializeField]
    GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = (_target.transform.position - transform.position).normalized;

        Quaternion rot = Quaternion.Euler(targetDir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 position = this.gameObject.transform.forward;
        Gizmos.DrawWireSphere(position, 1f);
    }
}
