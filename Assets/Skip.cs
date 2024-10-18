using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    //테스트를 위한 스크립트
    [SerializeField] private Transform _warpTransform;

    [SerializeField] private GameObject[] _testObject;

    private GameObject _player;

    private void Start()
    {
        _player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _player.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            TelePortTransform();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            AllSetActiveObject();
        }
    }

    private void TelePortTransform()
    {
        if(_warpTransform == null)
        {
            return;
        }

        _player.transform.position = _warpTransform.position;

        _player.transform.rotation = Quaternion.identity;
    }

    private void AllSetActiveObject()
    {
        foreach (var testObject in _testObject)
        {
            if (testObject != null)
            {
                testObject.SetActive(true);
            }
        }
    }
}
