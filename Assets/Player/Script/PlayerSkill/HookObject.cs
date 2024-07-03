using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Must;

public class HookObject : ProjectileObject, IHookPosition
{
    private Action<Vector3, bool> _hookEventHandler;
    private DrawSegment m_draw;
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
        m_draw = GetComponent<DrawSegment>();

        transform.gameObject.layer = 0;
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
        if (Vector3.Distance(transform.position, m_player.transform.position) >= m_maxDistance)
        {
            Debug.Log(Vector3.Distance(transform.position, m_player.transform.position));
            isFire = false;

            m_draw.SetIsFire(isFire);

            transform.gameObject.layer = 10;
        }   
    }

    private void DeleteSegment()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hook")
        {
            _hookEventHandler?.Invoke(transform.position, true);
            isAnchor = true;
        }
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!isFire)
            {
                _hookEventHandler?.Invoke(transform.position, false);
                PoolManager.Instance.ReturnHookObject(this.gameObject);
            }
            else
            {
                PoolManager.Instance.ReturnHookObject(this.gameObject);
            }
            
        }
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
