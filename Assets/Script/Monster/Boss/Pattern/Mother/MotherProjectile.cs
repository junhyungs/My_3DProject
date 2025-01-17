using System.Collections;
using UnityEngine;

public class MotherProjectile : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private ForestMotherProjectileData _data;
    private MeshRenderer _meshRenderer;
    private Material _material;

    private Vector3 _targetDirection;
    private LayerMask _targetLayer;
    private LayerMask _explosionLayer;

    public Transform PlayerTransform { get; set; }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _meshRenderer = GetComponent<MeshRenderer>();

        _targetLayer = LayerMask.GetMask("Player");

        _explosionLayer = LayerMask.GetMask("Ground", "Player", "Wall");

        _data = BossManager.Instance.ProjectileData;
        
    }

    private void OnEnable()
    {
        _material = _meshRenderer.material;

        _material.SetFloat("_Float", 1f);
    }

    public void ProjectileMove()
    {
        _targetDirection = (PlayerTransform.position - transform.position).normalized;

        Vector3 moveTotarget = new Vector3(_targetDirection.x, _targetDirection.y + 3f,
            _targetDirection.z);

        _rigidBody.AddForce(moveTotarget * _data.ForcePower, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //비트 마스크 방식. 왼쪽으로 other.gameObject.layer만큼 이동.
        if (((1 << collision.gameObject.layer) & _explosionLayer) != 0)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        _material.SetFloat("_Float", 0f);

        ParticleSystem explosion = transform.GetChild(0).GetComponent<ParticleSystem>();
        explosion.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, _data.SphereRadius, _targetLayer);

        foreach(var target in  colliders)
        {
            if (_data.LayerList.Contains(target.gameObject.layer))
            {
                IDamaged hit = target.GetComponent<IDamaged>();

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
