using UnityEngine;

public class Stove : MonoBehaviour, IBurningObject
{
    [Header("FireParticle")]
    [SerializeField] private GameObject m_BigfireParticleObject;

    [Header("OnFire")]
    [SerializeField] private bool isBurning;

    public bool IsBurning
    {
        get { return isBurning; }
    }

    private void Start()
    {
        if (isBurning)
        {
            m_BigfireParticleObject.SetActive(true);
        }
    }

    public void OnBurning(bool isBurning)
    {
        if(m_BigfireParticleObject == null ||
            this.isBurning)
        {
            return;
        }

        m_BigfireParticleObject.SetActive(isBurning);

        this.isBurning = isBurning;
    }
}
