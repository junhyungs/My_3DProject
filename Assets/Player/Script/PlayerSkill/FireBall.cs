using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    private void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.FireBall);
    }
    
    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject fireBall = PoolManager.Instance.GetFireBall();
        GameObject fireBallParticle = fireBall.transform.GetChild(0).gameObject;

        FireBallObject fireBallComponent = fireBall.GetComponent<FireBallObject>();
        fireBallComponent.IsFire(false);
        fireBallComponent.SetProjectileObjectData(m_skillData.m_attackPower, m_skillData.m_projectileSpeed, m_skillData.m_projectileRange);
        fireBall.transform.SetParent(spawnPositionObj.transform);
        fireBall.transform.localPosition = Vector3.zero;
        spawnPositionObj.transform.localRotation = Quaternion.identity; 
        fireBall.transform.localRotation = spawnPositionObj.transform.localRotation;
        fireBallParticle.SetActive(true);
        
    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        if(spawnPositionObj.transform.childCount != 0)
        {
            GameObject fireBall = spawnPositionObj.transform.GetChild(0).gameObject;
            FireBallObject fireBallComponent = fireBall.GetComponent<FireBallObject>();
            fireBallComponent.IsFire(isFire);
            fireBall.transform.parent = null;
        }
    }
}
