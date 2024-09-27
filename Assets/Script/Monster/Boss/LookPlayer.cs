using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookPlayer : MonoBehaviour
{
    private Rig _upperAim;
    private Transform _playerTransform;

    private void Awake()
    {
        _upperAim = gameObject.GetComponent<Rig>();
    }

    void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
    }

    void Update()
    {
        //Debug.Log(Vector3.Distance(transform.position, _playerTransform.position));

        //float distance = Vector3.Distance(transform.position, _playerTransform.position);

        //if(distance <= 11f)
        //{
        //    _upperAim.weight = Mathf.Lerp(1f, 0f, (11f - distance) / 11f);
        //}
        //else
        //{
        //    _upperAim.weight = 1f;
        //}
    }
}
