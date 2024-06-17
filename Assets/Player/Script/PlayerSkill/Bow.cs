using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Skill
{
    private GameObject Arrow;

    private void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.Bow);
    }

    public override void UseSkill(Transform spawnPosition)
    {
        Arrow = PoolManager.Instance.GetArrow();
        ArrowObject arrowObj = Arrow.GetComponent<ArrowObject>();
        arrowObj.IsFire(false);
        arrowObj.SetProjectileObjectData(m_skillData.m_attackPower, m_skillData.m_projectileSpeed, m_skillData.m_projectileRange);
        Arrow.transform.position = spawnPosition.position;
        Arrow.transform.SetParent(spawnPosition);
    }

    public override void Fire(bool isFire)
    {
        ArrowObject arrowObj = Arrow.GetComponent<ArrowObject>();
        arrowObj.IsFire(isFire);
        Arrow.transform.parent = null;
        Arrow = null;
    }
}
