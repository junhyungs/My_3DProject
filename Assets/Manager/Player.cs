using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine;


public class Player : MonoBehaviour
{
    private float m_Soul;

    private void Awake()
    {
        GameManager.Instance.Player = this.gameObject;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Soul")
        {
            DropSoul soul = other.gameObject.GetComponent<DropSoul>();

            m_Soul += soul.GetSoulValue();
        }
    }
}
