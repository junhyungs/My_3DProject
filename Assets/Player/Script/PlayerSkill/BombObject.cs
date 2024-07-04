using System.Collections;
using UnityEngine;

public class BombObject : ProjectileObject
{
    private MeshRenderer m_bombMeshRenderer;
    private Material m_bombMaterial;
    private int m_maxBounce = 2;
    private int m_currentBounce;
    private float m_forceTime;
    private float m_forcePowerTime;
    private float m_maxPower = 30.0f;
    private bool isFlying;

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

        m_currentBounce = 0;

        m_forceTime = 0.3f;
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;

        if(isFire)
        {
            Invoke(nameof(ReturnBomb), 4f);
            isFlying = true;
        }
    }

    private void BombUp(float forcePowerTime)
    {
        float forcePower = m_speed * forcePowerTime;

        if(forcePower > m_maxPower)
        {
            forcePower = 30.0f;
        }

        Vector3 moveDirection = transform.TransformDirection(new Vector3(0, 2, 1).normalized);

        m_projectileRigidbody.useGravity = true;

        m_projectileRigidbody.AddForce(moveDirection * forcePower, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if(!isFlying)
            m_forcePowerTime += Time.deltaTime;

        if (isFlying && m_forceTime > 0f)
        {
            BombUp(m_forcePowerTime);

            m_forceTime -= Time.deltaTime;
        }

    }

    

    private void OnCollisionEnter(Collision collision)
    {
        m_currentBounce++;

        if(m_currentBounce < m_maxBounce)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                Explosion();
            }
        }
        else
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        m_bombMaterial.SetFloat("_Float", 0);

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
        PoolManager.Instance.ReturnBombObject(this.gameObject);
    }
}
