using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Skill
{

    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject hook = ObjectPool.Instance.DequeueObject(ObjectName.PlayerHook);

        HookObject hookComponent = hook.GetComponent<HookObject>();
        hookComponent.IsFire(false);
        hookComponent.SetProjectileObjectData(_data.Power, _data.ProjectileSpeed, _data.SkillRange);
        hook.transform.SetParent(spawnPositionObj.transform);
        hook.transform.localPosition = Vector3.zero;
        hook.transform.localRotation = spawnPositionObj.transform.localRotation;
    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        if(spawnPositionObj.transform.childCount != 0)
        {
            GameObject hook = spawnPositionObj.transform.GetChild(0).gameObject;
            HookObject hookComponent = hook.GetComponent<HookObject>();
            hookComponent.IsFire(true);
            hook.transform.parent = null;
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
