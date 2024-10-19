using UnityEngine;

public class ArrowObject : ProjectileObject
{
    [Header("FireParticle")]
    [SerializeField] private GameObject m_fireParticleObject;

    private bool isBurning = false;

    protected override void Awake()
    {
        base.Awake();      
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;

        if(isFire == true)
        {
            Invoke(nameof(ReturnArrow), 5.0f);
        }
    }

    private void FixedUpdate()
    {
        if (isFire)
        {
            Vector3 moveForce = transform.forward * m_speed;

            m_projectileRigidbody.AddForce(moveForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if(hit != null)
            {
                hit.TakeDamage(m_atk);

                ReturnArrow();
            }
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("HitSwitch"))
        {
            ReturnArrow();
        }

        Stove stove = other.gameObject.GetComponent<Stove>();

        if(stove == null)
        {
            return;
        }

        if (isBurning)
        {
            IBurningObject burningObject = stove.GetComponent<IBurningObject>();

            if(burningObject != null)
            {
                burningObject.OnBurning(true);
            }
        }
        else
        {
            bool burning = stove.IsBurning;

            OnFireParticle(burning);
        }
    }

    private void OnFireParticle(bool buring)
    {
        if (buring)
        {
            m_fireParticleObject.SetActive(true);
            this.isBurning = buring;
        }
    }

    private void ReturnArrow()
    {
        m_projectileRigidbody.velocity = Vector3.zero;

        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerArrow);
    }

}
