using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_BossChangeScene : MonoBehaviour
{
    public void ChangeScene()
    {
        LoadingScene.LoadScene("Room");
    }
}
