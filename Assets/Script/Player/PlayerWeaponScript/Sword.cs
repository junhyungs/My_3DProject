using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class Sword : Weapon
{
    public override void SetWeaponData(PlayerWeaponData weaponData)
    {
        _weaponData = weaponData;

        _targetLayer = LayerMask.GetMask("Monster", "HitSwitch");
    }

    public override void UseWeapon(bool isCharge)
    {
        _forward = transform.forward;

        _boxPosition = transform.position + _forward * 1.5f;

        _currentPower = isCharge ? _weaponData.ChargePower : _weaponData.Power;

        Vector3 boxSize = isCharge ? _weaponData.ChargeAttackRange : _weaponData.NormalAttackRange;

        Collider[] colliders = Physics.OverlapBox(_boxPosition, boxSize / 2, transform.rotation, _targetLayer);

        foreach(var target in  colliders)
        {
            Hit(target);
        }
    }

    private void Hit(Collider other)
    {
        IDamged damged = other.gameObject.GetComponent<IDamged>();

        if (damged != null)
        {
            damged.TakeDamage(_currentPower);

            float effectCount = _weaponData.EffectCount;

            for(int i = 0; i < effectCount; i++)
            {
                GameObject hitEffect = PoolManager.Instance.GetHitParticle();

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

        if(hitSwitch != null)
        {
            hitSwitch.SwitchEvent();
        }
    }
}
