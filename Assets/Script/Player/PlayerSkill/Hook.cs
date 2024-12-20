using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Skill
{
    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject hook = ObjectPool.Instance.DequeueObject(ObjectName.PlayerHook);
        //SetProjectile(spawnPositionObj, hook);
        HookObject hookComponent = hook.GetComponent<HookObject>();
        hookComponent.IsFire(false);
        hookComponent.SetProjectileObjectData(_data.Power, _data.ProjectileSpeed, _data.SkillRange);
        hook.transform.SetParent(spawnPositionObj.transform);

        hook.transform.localPosition = Vector3.zero;
        hook.transform.localRotation = spawnPositionObj.transform.localRotation;

        Vector3 startPosition = spawnPositionObj.transform.root.position;
        hookComponent.StartPosition = startPosition;

        //Debug.Log(startPosition);
        ////SetStartPosition(spawnPositionObj, hook);

        
    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        Shoot(spawnPositionObj, isFire);
    }

    private void SetStartPosition(GameObject spawnPositionObj, GameObject hook)
    {
        HookObject hookComponent = hook.GetComponent<HookObject>();

        Vector3 startPosition = spawnPositionObj.transform.localPosition;
        startPosition.z = 0f;
        startPosition = spawnPositionObj.transform.TransformPoint(startPosition);

        hookComponent.StartPosition = startPosition;
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
