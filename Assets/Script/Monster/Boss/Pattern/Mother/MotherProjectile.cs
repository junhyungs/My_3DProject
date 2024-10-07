using System.Collections;
using UnityEngine;

public class MotherProjectile : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Transform _playerTransform;
    private ForestMotherProjectileData _data;
    private MeshRenderer _meshRenderer;
    private Material _material;

    private Vector3 _targetDirection;

    private bool _explosion;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _playerTransform = GameManager.Instance.Player.transform;
        _data = BossManager.Instance.ProjectileData;
    }

    private void OnEnable()
    {
        _explosion = true;

        _material = _meshRenderer.material;

        _material.SetFloat("_Float", 1f);
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
        _material.SetFloat("_Float", 0f);

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

        _rigidBody.velocity = Vector3.zero; 
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.MotherProjectile);
    }
}
