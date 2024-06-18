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

    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        
    }
}
