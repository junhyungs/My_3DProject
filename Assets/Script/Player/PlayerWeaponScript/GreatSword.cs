using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{
    public override void SetWeaponData(PlayerWeaponData weaponData)
    {
        _weaponData = weaponData;
    }

    public override void UseWeapon(bool isCharge)
    {
        _targetLayer = LayerMask.GetMask("Monster", "HitSwitch");

        _forward = transform.forward;

        _boxPosition = transform.position +_forward + Vector3.up * 0.6f;

        _currentPower = isCharge ? _weaponData.ChargePower : _weaponData.Power;

        Vector3 boxSize = isCharge ? _weaponData.ChargeAttackRange : _weaponData.NormalAttackRange;

        Collider[] colliders = Physics.OverlapBox(_boxPosition, boxSize / 2, transform.rotation, _targetLayer);

        if (colliders.Length <= 0)
        {
            return;
        }

        foreach (var target in colliders)
        {
            AttackTarget(target);
        }
    }

    private void AttackTarget(Collider other)
    {
        IDamged damged = other.gameObject.GetComponent<IDamged>();

        if (damged != null)
        {
            damged.TakeDamage(_currentPower);

            SkillManager.Instance.SkillCount++;

            float effectCount = _weaponData.EffectCount;

            for (int i = 0; i < effectCount; i++)
            {
                GameObject hitEffect = ObjectPool.Instance.DequeueObject(ObjectName.HitEffect);

                HitEffect hitEffectComponent = hitEffect.GetComponent<HitEffect>();

                ParticleSystem hitSystem = hitEffect.GetComponent<ParticleSystem>();

                int randomRot = UnityEngine.Random.Range(20, 161);

                hitEffect.transform.rotation = Quaternion.Euler(0, 0, randomRot);

                hitEffect.transform.position = other.transform.position;

                hitEffect.SetActive(true);

                hitSystem.Play();

                hitEffectComponent.ReturnEffect();
            }
        }

        HitSwitch hitSwitch = other.gameObject.GetComponent<HitSwitch>();

        if (hitSwitch != null)
        {
            hitSwitch.SwitchEvent();
        }
    }
}
