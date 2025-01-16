using UnityEngine;
using System;

public class DeathUI : MonoBehaviour
{
    [Header("DeathImage")]
    [SerializeField] private GameObject _deathText;

    private void OnEnable()
    {
        UIManager._deathUI += DeathImage;
    }

    private void OnDisable()
    {
        UIManager._deathUI -= DeathImage;
    }

    private void Start()
    {
        _deathText.SetActive(false);    
    }

    private void DeathImage(bool isActive)
    {
        Action uiAction = isActive ? OnEnableDeathUI : OnDisableDeathUI;

        uiAction.Invoke();
    }

    private void OnEnableDeathUI()
    {
        _deathText.SetActive(true);
    }

    private void OnDisableDeathUI()
    {
        _deathText.SetActive(false);
    }

}
