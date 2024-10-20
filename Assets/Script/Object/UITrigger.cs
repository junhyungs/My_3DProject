using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrigger : MonoBehaviour
{
    [Header("TriggerUI")]
    [SerializeField] private GameObject _ui;

    [Header("Time")]
    [SerializeField] private float _seconds;

    private bool _isTrigger;
    private WaitForSeconds _uiTriggerTime;

    private void Awake()
    {
        _uiTriggerTime = new WaitForSeconds(_seconds);
    }

    private void OnEnable()
    {
        _isTrigger = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!_isTrigger)
            {
                StartCoroutine(OnUI());
            }
        }
    }

    private IEnumerator OnUI()
    {
        _isTrigger = true;

        _ui.SetActive(true);

        yield return _uiTriggerTime;

        _ui.SetActive(false);
    }
}
