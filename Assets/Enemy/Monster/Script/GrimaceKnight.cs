using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimaceKnight : Monster
{
    public override void TakeDamage(float damage)
    {
        
    }

    protected override void Start()
    {
        base.Start();
        
        
    }

    private void Update()
    {
        if (Vector3.Distance(m_player.transform.position, transform.position) <= 2.0f)
            m_monsterAnim.SetTrigger("Bite");
    }


    

}
