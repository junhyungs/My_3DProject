using UnityEngine;

public class Stove : MonoBehaviour, IBurningObject
{
    [Header("BurningParticle")]
    [SerializeField] private ParticleSystem _burningParticle;

    [Header("OnFire")]
    [SerializeField] private bool _isBurning;

    private void Start()
    {
        if (_isBurning)
        {
            OnParticleSystem();
        }
    }

    public void OnBurning()
    {
        if(_isBurning)
        {
            return;
        }

        _isBurning = true;

        OnParticleSystem();
    }

    public bool IsBurning()
    {
        return _isBurning;
    }

    private void OnParticleSystem()
    {
        _burningParticle.gameObject.SetActive(true);

        _burningParticle.Play();
    }
}
