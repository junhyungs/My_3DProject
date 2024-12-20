using UnityEngine;

public class Bow : Skill
{
    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject arrowObj = ObjectPool.Instance.DequeueObject(ObjectName.PlayerArrow);
        
        SetProjectile(spawnPositionObj, arrowObj);
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
