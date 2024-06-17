using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Skill
{
    private void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.Hook);
    }

    public override void Fire(bool isFire)
    {
        
    }

    public override void UseSkill(Transform spawnPosition)
    {
        
    }    
}
