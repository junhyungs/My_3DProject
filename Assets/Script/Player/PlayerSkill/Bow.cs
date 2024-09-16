using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Skill
{
    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject arrowObj = PoolManager.Instance.GetArrow();
        
        ArrowObject arrowComponent = arrowObj.GetComponent<ArrowObject>();
        arrowComponent.IsFire(false);
        arrowComponent.SetProjectileObjectData(_data.Power, _data.ProjectileSpeed, _data.SkillRange);
        arrowObj.transform.SetParent(spawnPositionObj.transform);
        arrowObj.transform.localPosition = Vector3.zero;
        arrowObj.transform.localRotation = spawnPositionObj.transform.localRotation;
    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        if(spawnPositionObj.transform.childCount != 0)
        {
            GameObject arrowObj = spawnPositionObj.transform.GetChild(0).gameObject;
            ArrowObject arrowComponent = arrowObj.GetComponent<ArrowObject>();
            arrowComponent.IsFire(isFire);
            arrowObj.transform.parent = null;
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
