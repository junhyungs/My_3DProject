using System.Collections;
using UnityEngine;

public class BombObject : ProjectileObject
{
    private MeshRenderer m_bombMeshRenderer;
    private Material m_bombMaterial;

    protected override void Awake()
    {
        base.Awake();

        m_projectileRigidbody.useGravity = false;
    }

    private void OnEnable()
    {
        m_bombMeshRenderer = transform.GetChild(2).gameObject.GetComponent<MeshRenderer>();

        m_bombMaterial = m_bombMeshRenderer.material;

        m_bombMaterial.SetFloat("_Float", 1.0f);
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;

        if(isFire)
        {
            Invoke(nameof(ReturnBomb), 4f);

            m_projectileRigidbody.useGravity = true;
        }
    }

    private void FixedUpdate()
    {
        if (isFire)
        {
            Vector3 moveDirection = transform.forward * m_speed;

            m_projectileRigidbody.AddForce(moveDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        m_bombMaterial.SetFloat("_Float", 0);

        isFire = false;

        m_projectileRigidbody.velocity = Vector3.zero;

        GameObject explosionParticle = gameObject.transform.GetChild(1).gameObject;

        explosionParticle.SetActive(true);

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_range, LayerMask.GetMask("Monster"));

        for(int i = 0; i < colliders.Length; i++)
        {
            IDamged hit = colliders[i].gameObject.GetComponent<IDamged>();

            if (hit != null)
            {
                hit.TakeDamage(m_atk);
            }
        }

        StartCoroutine(ReturnPool());
    }

    private IEnumerator ReturnPool()
    {
        yield return new WaitForSeconds(3.5f);

        ReturnBomb();
    }

    private void ReturnBomb()
    {
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerBomb);
    }
}
