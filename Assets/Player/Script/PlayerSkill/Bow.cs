using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Skill
{
    private void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.Bow);
    }

    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject arrowObj = PoolManager.Instance.GetArrow();
        
        ArrowObject arrowComponent = arrowObj.GetComponent<ArrowObject>();
        arrowComponent.IsFire(false);
        arrowComponent.SetProjectileObjectData(m_skillData.m_attackPower, m_skillData.m_projectileSpeed, m_skillData.m_projectileRange);
        arrowObj.transform.SetParent(spawnPositionObj.transform);
        arrowObj.transform.localPosition = Vector3.zero;
        arrowObj.transform.localRotation = spawnPositionObj.transform.localRotation;
        
    }


    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        GameObject arrowObj = spawnPositionObj.transform.GetChild(0).gameObject;
        ArrowObject arrowComponent = arrowObj.GetComponent<ArrowObject>();
        arrowComponent.IsFire(isFire);
        arrowObj.transform.parent = null;
    }
}
