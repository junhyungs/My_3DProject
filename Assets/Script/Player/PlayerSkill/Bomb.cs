using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Skill
{
    public override void UseSkill(GameObject spawnPositionObj)
    {
        GameObject bomb = ObjectPool.Instance.DequeueObject(ObjectName.PlayerBomb);
        GameObject bombParticle = bomb.transform.GetChild(0).gameObject;

        BombObject bombComponent = bomb.GetComponent<BombObject>();
        bombComponent.IsFire(false);
        bombComponent.SetProjectileObjectData(_data.Power, _data.ProjectileSpeed, _data.SkillRange);
        bomb.transform.SetParent(spawnPositionObj.transform);
        bomb.transform.localPosition = Vector3.zero;

        spawnPositionObj.transform.localRotation = Quaternion.identity;
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

    public override void SetSkillData(PlayerSkillData skillData)
    {
        _data = skillData;
    }

    public override PlayerSkillData GetSkillData()
    {
        return _data;
    }
}
