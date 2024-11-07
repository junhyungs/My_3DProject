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
        HitMonster(other);
        HitSwitch(other);
        HitBurningObject(other);
    }

    private void HitMonster(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IDamged damaged = other.gameObject.GetComponent<IDamged>();

            if(damaged != null)
            {
                damaged.TakeDamage(m_atk);

                ReturnArrow();
            }
        }
    }

    private void HitSwitch(Collider other)
    {
        IHitSwitch hitSwitch = other.gameObject.GetComponent<IHitSwitch>();

        if(hitSwitch != null)
        {
            hitSwitch.OnHitSwitch();
        }
    }

    private void HitBurningObject(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BurningObject"))
        {
            IBurningObject burningObject = other.gameObject.GetComponent<IBurningObject>();

            if (isBurning)
            {
                burningObject.OnBurning();
            }
            else
            {
                bool setBurning = burningObject.IsBurning();

                if (setBurning)
                {
                    OnParticleSystem();
                }
            }
        }
    }

    private void OnParticleSystem()
    {
        m_fireParticleObject.SetActive(true);

        isBurning = true;
    }

    private void ReturnArrow()
    {
        m_projectileRigidbody.velocity = Vector3.zero;

        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerArrow);
    }

}
