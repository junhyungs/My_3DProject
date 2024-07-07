using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class R_BossGimikManager : MonoBehaviour
{
    [Header("HitSwitch_1")]
    [SerializeField] private HitSwitch m_hitSwitch_1;

    [Header("HitSwitch_2")]
    [SerializeField] private HitSwitch m_hitSwitch_2;

    [Header("HitSwitch_3")]
    [SerializeField] private HitSwitch m_hitSwitch_3;

    private bool isClear;

    [Header("BossDoor")]
    [SerializeField] private GameObject m_BossDoor;

    private void Update()
    {
        OnBossRoom();
    }

    private void OnBossRoom()
    {
         isClear = m_hitSwitch_1.IsHit && m_hitSwitch_2.IsHit
            && m_hitSwitch_3.IsHit;

        if (isClear)
        {
            isClear = false;
            var gimik = GimikManager.Instance.Gimik;

            if(gimik.TryGetValue(1,out Action<GameObject> gimikEvent))
            {
                gimikEvent.Invoke(m_BossDoor);
            }
        }
    }

}
