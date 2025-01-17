using UnityEngine;
using System;

public class HookObject : ProjectileObject, IHookPosition
{
    private Action<Vector3, bool> _hookEventHandler;
    private Vector3 m_movePos;

    public Vector3 StartPosition { get; set; }
    private float m_maxDistance = 10.0f;
    private bool isAnchor;

    private void OnEnable()
    {
        EventManager.Instance.RegisterHookPositionEvent(this);
    
        transform.gameObject.layer = LayerMask.NameToLayer("Default");
        isAnchor = false;
    }

    private void OnDisable()
    {
        _hookEventHandler = null;
    }


    public override void IsFire(bool fire)
    {
        _isFire = fire;
    }

    public override void SetProjectileObjectData(float atk, float speed, float range)
    {
        _projectileAtk = atk;
        _projectileSpeed = speed;
        _range = range;
    }

    private void Update()
    {
        if (isAnchor)
        {
            return;
        }

        FireDistance();

        if (_isFire)
        {
            m_movePos = transform.forward * (_projectileSpeed + 10) * Time.deltaTime;
            transform.position += m_movePos;
        }
        else
        {
            m_movePos = -transform.forward * (_projectileSpeed + 10) * Time.deltaTime;
            transform.position += m_movePos;
        }
    }

    private void FireDistance()
    {

        float currentDistance = Vector3.Distance(transform.position, StartPosition);

        if (currentDistance >= m_maxDistance && !isAnchor)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("Hook"));

            foreach(var obj in colliders)
            {
                GameObject chain = obj.gameObject;
                ObjectPool.Instance.EnqueueObject(chain, ObjectName.PlayerSegment);
            }

            _isFire = false;
            transform.gameObject.layer = LayerMask.NameToLayer("Player");
        } 
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hook")
        {
            _hookEventHandler.Invoke(transform.position, true);
            isAnchor = true;
            ReturnHook();
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Monster") && _isFire)
        {
            IDamaged hit = other.gameObject.GetComponent<IDamaged>();

            if (hit != null)
            {
                hit.TakeDamage(_projectileAtk);
            }

            _hookEventHandler?.Invoke(transform.position, true);

            isAnchor = true;
            ReturnHook();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if (!_isFire)
            {
                _hookEventHandler.Invoke(transform.position, false);

                ReturnHook();
            }
            else
            {
                ReturnHook();
            }

        }
    }

    private void ReturnHook()
    {
        ObjectPool.Instance.EnqueueObject(this.gameObject, ObjectName.PlayerHook);
    }

    public void HookPositionEvent(bool isAddEvent, Action<Vector3, bool> callBack)
    {
        if (isAddEvent)
        {
            _hookEventHandler += callBack;
        }
        else
        {
            _hookEventHandler -= callBack;
        }
    }
}
