using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScenePlayer : MonoBehaviour
{
    private Animator m_startScenePlayerAnim;

    private WaitForSeconds m_stretchTime = new WaitForSeconds(50.0f);

    void Start()
    {
        m_startScenePlayerAnim = GetComponent<Animator>();
        StartCoroutine(StartPlayerAnimation());
    }

    IEnumerator StartPlayerAnimation()
    {
        while (true)
        {
            yield return m_stretchTime;

            m_startScenePlayerAnim.SetTrigger("Stretch");
        }
    }
}
