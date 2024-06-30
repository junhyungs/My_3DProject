using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine;


public class Player : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.Player = this.gameObject;
    }

}
