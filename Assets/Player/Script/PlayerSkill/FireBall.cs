using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    private void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.FireBall);
    }
    
    public override void UseSkill(Transform spawnPosition)
    {
        
    }

    public override void Fire(bool isFire)
    {
        
    }
}
