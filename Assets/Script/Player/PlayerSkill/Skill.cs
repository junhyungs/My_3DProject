using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Skill : MonoBehaviour, ISkill
{
    protected SkillData m_skillData;
    public abstract void UseSkill(GameObject spawnPositionObj);
    public abstract void Fire(GameObject spawnPositionObj, bool isFire);
}
