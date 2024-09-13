
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    private float m_defaultAtk;
    private float m_chargeAtk;
    private float m_currentAtk;
    private Vector3 m_defaultRange;
    private Vector3 m_chargeRange;
    private Vector3 m_boxposition;
    private Vector3 m_objectforward;
    private MeshCollider m_triggerCollider;
    private int m_effectCount = 3;


    private void Start()
    {
        EventManager.Instance.AddEvent_ActiveTriggerColliderEvent(true, OnCollider);
        EventManager.Instance.AddEvent_DeActiveTriggerColliderEvent(true, OffCollider);
        EventManager.Instance.AddEvent_OverlapBoxEvent(true, HitOverlapBox);
        EventManager.Instance.AddEvent_SetWeaponDataEvent(true, SetWeaponData);
        m_triggerCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        m_triggerCollider.enabled = false;
    }

    public void OnCollider()
    {
        m_triggerCollider.enabled = true;
    }

    public void OffCollider()
    {
        m_triggerCollider.enabled = false;
    }

  
    public void SetWeaponData(float defaultAtk, float chargeAtk, Vector3 defaultRange, Vector3 chargeRange)
    {
        m_defaultAtk = defaultAtk;
        m_chargeAtk = chargeAtk;
        m_defaultRange = defaultRange;
        m_chargeRange = chargeRange;
    }

    public void HitOverlapBox(bool isCharge)
    {
        m_objectforward = transform.forward;

        m_boxposition = transform.position + m_objectforward * 1.5f;

        m_currentAtk = isCharge ? m_chargeAtk : m_defaultAtk;
        
        Vector3 boxSize = isCharge ? m_chargeRange : m_defaultRange;

        Collider[] boxOverlap = Physics.OverlapBox(m_boxposition, boxSize / 2, transform.rotation, LayerMask.GetMask("Monster", "HitSwitch"));

        foreach (var box in boxOverlap)
        {
            Hit(box);
        }
    }

    private void Hit(Collider other)
    {
        IDamged damged = other.gameObject.GetComponent<IDamged>();        
        HitSwitch hitSwitch = other.gameObject.GetComponent<HitSwitch>();   

        if (damged != null)
        {
            damged.TakeDamage(m_currentAtk);

            for(int i = 0; i < m_effectCount; i++)
            {
                GameObject hitEffect = PoolManager.Instance.GetHitParticle();

                HitEffect hitEffectComponent = hitEffect.GetComponent<HitEffect>();

                ParticleSystem hitSystem = hitEffect.GetComponent<ParticleSystem>();

                int randomRot = Random.Range(20, 161);

                hitEffect.transform.rotation = Quaternion.Euler(0, 0, randomRot);

                hitEffect.transform.position = other.transform.position;

                hitEffect.SetActive(true);

                hitSystem.Play();

                hitEffectComponent.ReturnEffect();
            }
        }

        if(hitSwitch != null)
        {
            hitSwitch.SwitchEvent();
        }
    }

}
