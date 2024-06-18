using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Skill
{
    void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.Bomb);
    }

    public override void UseSkill(GameObject spawnPositionObj)
    {
     
    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
     
    }
}
