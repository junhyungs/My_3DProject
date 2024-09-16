using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{    
    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject fireBall = PoolManager.Instance.GetFireBall();
        GameObject fireBallParticle = fireBall.transform.GetChild(0).gameObject;

        FireBallObject fireBallComponent = fireBall.GetComponent<FireBallObject>();
        fireBallComponent.IsFire(false);
        fireBallComponent.SetProjectileObjectData(_data.Power, _data.ProjectileSpeed, _data.SkillRange);
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

    public override void SetSkillData(PlayerSkillData skillData)
    {
        _data = skillData;
    }

    public override PlayerSkillData GetSkillData()
    {
        return _data;   
    }
}
