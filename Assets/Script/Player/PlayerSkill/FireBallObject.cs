using UnityEngine;

public class FireBallObject : ProjectileObject
{
    private int m_piercingPower = 5;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        m_piercingPower = 5;
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;

        if(isFire == true)
        {
            Invoke(nameof(ReturnFireBall), 5.0f);
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
        Piercing(other);
        HitMonster(other);
        HitSwitch(other);
        HitBurningObject(other);
    }

    private void Piercing(Collider other)
    {
        if(m_piercingPower <= 0 || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ReturnFireBall();
        }
    }

    private void HitMonster(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if (hit != null)
            {
                hit.TakeDamage(m_atk);

                m_piercingPower--;
            }
        }
    }

    private void HitSwitch(Collider other)
    {
        IHitSwitch hitSwitch = other.gameObject.GetComponent<IHitSwitch>();

        if (hitSwitch != null)
        {
            hitSwitch.OnHitSwitch();
        }
    }

    private void HitBurningObject(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BurningObject"))
        {
            IBurningObject burningObject = other.gameObject.GetComponent<IBurningObject>();

            if(burningObject != null)
            {
                burningObject.OnBurning();
            }
        }
    }

    private void ReturnFireBall()
    {
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerFireBall);
    }

    
}
