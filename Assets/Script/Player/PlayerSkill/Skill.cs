using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Skill : ISkill
{
    protected PlayerSkillData _data; 
    public abstract void SetSkillData(PlayerSkillData skillData);
    public abstract void UseSkill(GameObject spawnPositionObj);
    public abstract void Fire(GameObject spawnPositionObj, bool isFire);
    public abstract PlayerSkillData GetSkillData();
}
