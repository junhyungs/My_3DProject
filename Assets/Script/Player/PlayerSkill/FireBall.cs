using UnityEngine;

public class FireBall : Skill
{    
    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject fireBall = ObjectPool.Instance.DequeueObject(ObjectName.PlayerFireBall);
        SetProjectile(spawnPositionObj, fireBall);

        GameObject fireBallParticle = fireBall.transform.GetChild(0).gameObject;
        fireBallParticle.SetActive(true);
    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        Shoot(spawnPositionObj, isFire);
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
