using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Skill
{
    private void Start()
    {
        m_skillData = SkillManager.Instance.GetSkillData(PlayerSkill.Bow);
    }

    public override void UseSkill()
    {
        Debug.Log("활 사용중");
    }
}
