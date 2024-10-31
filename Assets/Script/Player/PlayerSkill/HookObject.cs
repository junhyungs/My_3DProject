using UnityEngine;
using System;

public class HookObject : ProjectileObject, IHookPosition
{
    private Action<Vector3, bool> _hookEventHandler;
    
    private GameObject m_player;
    private Vector3 m_movePos;
    private float m_maxDistance = 10.0f;
    private bool isAnchor;

    private void OnEnable()
    {
        EventManager.Instance.RegisterHookPositionEvent(this);

        m_player = GameManager.Instance.Player;
        transform.gameObject.layer = LayerMask.NameToLayer("Default");
        isAnchor = false;
    }

    private void OnDisable()
    {
        _hookEventHandler = null;
    }

    public override void IsFire(bool fire)
    {
        isFire = fire;
    }

    void FixedUpdate()
    {
        if (isAnchor)
        {
            return;
        }

        FireDistance();

        if (isFire)
        {
            m_movePos = transform.forward * (m_speed + 10) * Time.fixedDeltaTime;
            transform.position += m_movePos;
        }
        else
        {
            m_movePos = -transform.forward * (m_speed + 10) * Time.fixedDeltaTime;
            transform.position += m_movePos;
        }

    }

    private void FireDistance()
    {

        float currentDistance = Vector3.Distance(transform.position, m_player.transform.position);

        if (currentDistance >= m_maxDistance && !isAnchor)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("Hook"));

            foreach(var obj in colliders)
            {
                GameObject chain = obj.gameObject;
                ObjectPool.Instance.EnqueueObject(chain, ObjectName.PlayerSegment);
            }

            isFire = false;
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

        if(other.gameObject.layer == LayerMask.NameToLayer("Monster") && isFire)
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if (hit != null)
            {
                hit.TakeDamage(m_atk);
            }

            _hookEventHandler?.Invoke(transform.position, true);

            isAnchor = true;
            ReturnHook();
        }
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if (!isFire)
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
