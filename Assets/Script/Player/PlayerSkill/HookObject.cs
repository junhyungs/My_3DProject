using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;

public class HookObject : ProjectileObject, IHookPosition
{
    private Action<Vector3, bool> _hookEventHandler;
    
    private GameObject m_player;
    private Vector3 m_movePos;
    private float m_maxDistance = 10.0f;
    private bool isAnchor;

    protected override void Awake()
    {
        EventManager.Instance.RegisterHookPositionEvent(this);
    }

    private void OnEnable()
    {
        m_player = GameManager.Instance.Player;
        transform.gameObject.layer = LayerMask.NameToLayer("Default");
        isAnchor = false;
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
        if (Vector3.Distance(transform.position, m_player.transform.position) >= m_maxDistance && !isAnchor)
        {
            isFire = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("Hook"));

            foreach(var obj in colliders)
            {
                GameObject chain = obj.gameObject;

                PoolManager.Instance.ReturnPlayerSegment(chain);
            }

            transform.gameObject.layer = LayerMask.NameToLayer("Player");
        }   
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hook")
        {
            _hookEventHandler.Invoke(transform.position, true);

            ReturnHook();

            isAnchor = true;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IDamged hit = other.gameObject.GetComponent<IDamged>();

            if (hit != null)
            {
                hit.TakeDamage(m_atk);
            }

            _hookEventHandler?.Invoke(transform.position, true);

            ReturnHook();

            isAnchor = true;
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
        PoolManager.Instance.ReturnHookObject(this.gameObject);
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