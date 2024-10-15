using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    [SerializeField] Transform[] Point;
    [SerializeField] GameObject BorderPrefab;
    [SerializeField] GameObject border1;
    // Start is called before the first frame update
    void Start()
    {
        border1 = Instantiate(BorderPrefab);
        var boxCol = border1.GetComponent<BoxCollider>();
        border1.transform.SetParent(transform);

        float distance = Vector3.Distance(Point[0].position, Point[1].position);
        Vector3 size = boxCol.size;
        size.x = distance;
        size.y = 2f;
        size.z = 0.1f;

        boxCol.size = size;
        Vector3 centerPos = (Point[0].position + Point[1].position) / 2;
        border1.transform.position = centerPos;
    }
}
