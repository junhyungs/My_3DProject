using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCollider : MonoBehaviour
{
    [Header("Height")]
    [SerializeField] private float _height;

    private Transform[] _objectTransform;
    private Transform _colTransform;

    private void Awake()
    {        
        {
            int colLength = transform.childCount;

            _objectTransform = new Transform[colLength];

            int index = 0;

            foreach (Transform childTransform in transform)
            {
                _objectTransform[index++] = childTransform;
            }

            var parnet = new GameObject("Collider");
            parnet.transform.SetParent(transform);
            _colTransform = parnet.transform;
            _colTransform.localPosition = Vector3.zero;
        }
    }

    private void Start()
    {
        CreateBoxCollider();
    }

    private void CreateBoxCollider()
    {
        for (int i = 0; i < _objectTransform.Length - 1; i++)
        {
            var centerObject = new GameObject();

            Vector3 center = (_objectTransform[i].position + _objectTransform[i + 1].position) / 2;

            centerObject.transform.position = center;

            float distance = Vector3.Distance(_objectTransform[i].position, _objectTransform[i + 1].position);

            BoxCollider centerCollider = centerObject.AddComponent<BoxCollider>();

            Vector3 size = centerCollider.size;

            size.x = distance;

            if(_height != 0)
            {
                size.y = _height;
            }
            else
            {
                size.y = 1f;
            }

            size.z = 0.1f;

            centerCollider.size = size;

            Vector3 lookVec = _objectTransform[i].position - _objectTransform[i+1].position;

            centerObject.transform.rotation = Quaternion.LookRotation(lookVec);
            centerObject.transform.Rotate(0, 90, 0);
            centerObject.transform.SetParent(_colTransform);
        }
    }

    
}
