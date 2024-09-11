using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Skill
{
    private void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.Hook);
    }

    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject hook = PoolManager.Instance.GetHook();

        HookObject hookComponent = hook.GetComponent<HookObject>();
        hookComponent.IsFire(false);
        hookComponent.SetProjectileObjectData(m_skillData.m_attackPower, m_skillData.m_projectileSpeed, m_skillData.m_projectileRange);
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
}
