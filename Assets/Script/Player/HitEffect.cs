using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private float m_returnTime = 2.0f;

    public void ReturnEffect()
    {
        Invoke(nameof(ReturnPool), m_returnTime);
        
    }

    private void ReturnPool()
    {
        PoolManager.Instance.ReturnHitParticle(this.gameObject);
    }
}
