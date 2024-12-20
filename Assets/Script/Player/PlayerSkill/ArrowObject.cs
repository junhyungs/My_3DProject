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
        InvokeReturnMethod(fire, nameof(ReturnArrow));
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
                damaged.TakeDamage(_projectileAtk);

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
