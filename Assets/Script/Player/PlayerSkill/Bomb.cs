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
        GameObject bomb = PoolManager.Instance.GetBomb();
        GameObject bombParticle = bomb.transform.GetChild(0).gameObject;

        BombObject bombComponent = bomb.GetComponent<BombObject>();
        bombComponent.IsFire(false);
        bombComponent.SetProjectileObjectData(m_skillData.m_attackPower, m_skillData.m_projectileSpeed, m_skillData.m_projectileRange);
        bomb.transform.SetParent(spawnPositionObj.transform);
        bomb.transform.localPosition = Vector3.zero;
        bomb.transform.localRotation = spawnPositionObj.transform.localRotation;

        bombParticle.SetActive(true);
        ParticleSystem bombParticleSystem = bombParticle.GetComponent<ParticleSystem>();
        bombParticleSystem.Play();
    }

    public override void Fire(GameObject spawnPositionObj, bool isFire)
    {
        if(spawnPositionObj.transform.childCount != 0)
        {
            GameObject bomb = spawnPositionObj.transform.GetChild(0).gameObject;
            BombObject bombComponent = bomb.GetComponent<BombObject>();
            bombComponent.IsFire(isFire);
            bomb.transform.parent = null;
        }
    }
}
