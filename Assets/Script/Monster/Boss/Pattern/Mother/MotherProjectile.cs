using System.Collections;
using UnityEngine;

public class MotherProjectile : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Transform _playerTransform;
    private ForestMotherProjectileData _data;

    private Vector3 _targetDirection;

    private bool _explosion;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _playerTransform = GameManager.Instance.Player.transform;
        _data = BossManager.Instance.ProjectileData;
    }

    private void OnEnable()
    {
        _explosion = true;
    }

    public void ProjectileMove()
    {
        _targetDirection = (_playerTransform.position - transform.position).normalized;

        Vector3 moveTotarget = new Vector3(_targetDirection.x, _targetDirection.y + 3f,
            _targetDirection.z);

        _rigidBody.AddForce(moveTotarget * _data.ForcePower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_explosion)
        {
            Explosion();

            _explosion = false;
        }
    }

    private void Explosion()
    {
        ParticleSystem explosion = transform.GetChild(0).GetComponent<ParticleSystem>();
        explosion.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _data.SphereRadius);

        foreach(var target in  colliders)
        {
            if (_data.LayerList.Contains(target.gameObject.layer))
            {
                IDamged hit = target.GetComponent<IDamged>();

                if (hit != null)
                {
                    hit.TakeDamage(_data.Damage);
                }
            }
        }

        StartCoroutine(Return());
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(2.0f);

        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.MotherProjectile);
    }
}
