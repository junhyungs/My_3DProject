using UnityEngine;

public abstract class Skill : ISkill
{
    protected PlayerSkillData _data; 

    public abstract void SetSkillData(PlayerSkillData skillData);
    public abstract void UseSkill(GameObject spawnPositionObj);
    public abstract void Fire(GameObject spawnPositionObj, bool isFire);
    public abstract PlayerSkillData GetSkillData();

    protected void SetProjectile(GameObject spawnPositionObj, GameObject projectile)
    {
        ProjectileObject projectileComponent = projectile.GetComponent<ProjectileObject>();
        projectileComponent.IsFire(false);
        projectileComponent.SetProjectileObjectData(_data.Power, _data.ProjectileSpeed, _data.SkillRange);

        spawnPositionObj.transform.localRotation = Quaternion.identity;

        projectile.transform.SetParent(spawnPositionObj.transform);
        projectile.transform.localPosition = Vector3.zero;
        projectile.transform.localRotation = spawnPositionObj.transform.localRotation;
    }

    protected void Shoot(GameObject spawnPositionObj, bool isFire)
    {
        if(spawnPositionObj.transform.childCount == 0)
        {
            return;
        }

        GameObject projectile = spawnPositionObj.transform.GetChild(0).gameObject;

        ProjectileObject projectileComponent = projectile.GetComponent<ProjectileObject>();

        projectileComponent.IsFire(isFire);

        projectile.transform.parent = null;
    }
}
