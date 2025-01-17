using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    protected PlayerWeaponEffectController m_weaponEffect;
    protected PlayerWeaponData _weaponData;
    protected PlayerData _playerData;

    protected Vector3 _forward;
    protected Vector3 _boxPosition;

    protected LayerMask _targetLayer;
    protected float _currentPower;

    public abstract void SetWeaponData(PlayerWeaponData weaponData, PlayerData playerData);
    public abstract void UseWeapon(bool isCharge);

    protected void FindTarget(bool isCharge)
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
            Attack(target);
        }
    }

    private void Attack(Collider other)
    {
        IDamaged damged = other.gameObject.GetComponent<IDamaged>();

        if (damged != null)
        {
            damged.TakeDamage(_currentPower + _playerData.Power);

            SkillManager.Instance.SkillCount++;

            float effectCount = _weaponData.EffectCount;

            for (int i = 0; i < effectCount; i++)
            {
                GameObject hitEffect = ObjectPool.Instance.DequeueObject(ObjectName.HitEffect);

                HitEffect hitEffectComponent = hitEffect.GetComponent<HitEffect>();

                ParticleSystem hitSystem = hitEffect.GetComponent<ParticleSystem>();

                int randomRot = UnityEngine.Random.Range(20, 161);

                hitEffect.transform.rotation = Quaternion.Euler(0, 0, randomRot);

                hitEffect.transform.position = other.transform.position + new Vector3(0f, 0.5f, 0f);

                hitEffect.SetActive(true);

                hitSystem.Play();

                hitEffectComponent.ReturnEffect();
            }
        }

        IHitSwitch hitSwitch = other.gameObject.GetComponent<IHitSwitch>();

        if (hitSwitch != null)
        {
            hitSwitch.OnHitSwitch();
        }
    }

    protected void DrawGizmos(Vector3 boxSize)
    {
        Vector3 boxPosition = transform.position + transform.forward + Vector3.up * 0.6f;

        Gizmos.color = Color.red;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxPosition, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        Gizmos.matrix = Matrix4x4.identity;
    }
}


