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
        InvokeReturnMethod(fire, nameof(ReturnFireBall));
    }
    public override void SetProjectileObjectData(float atk, float speed, float range)
    {
        _projectileAtk = atk;
        _projectileSpeed = speed;
        _range = range;
    }

    private void FixedUpdate()
    {
        if (_isFire)
        {
            Vector3 moveForce = transform.forward * _projectileSpeed;

            m_projectileRigidbody.AddForce(moveForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_piercingPower <= 0 || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ReturnFireBall();

            return;
        }

        HitMonster(other);
        HitSwitch(other);
        HitBurningObject(other);
    }

    private void HitMonster(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if (hit != null)
            {
                hit.TakeDamage(_projectileAtk);

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
        m_projectileRigidbody.velocity = Vector3.zero;

        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerFireBall);
    }
}
