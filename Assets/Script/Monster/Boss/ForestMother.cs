using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ForestMother : MonoBehaviour, IDamged
{
    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderer;
    [Header("LowerBodyMesh")]
    [SerializeField] private MeshRenderer _lowerBody;
    [Header("Material")]
    [SerializeField] private Material _material;

    #region Component
    private Material _copyMaterial;
    private WaitForSeconds _intensityTime = new WaitForSeconds(0.1f);
    private Animator _animator;
    public ForestMotherProperty Property { get; set; }
    #endregion

    private void Awake()
    {
        InitializeForestMother();
    }

    private void OnEnable()
    {
        var materialValue = _copyMaterial.GetFloat("_Float");

        if (materialValue <= 0f)
        {
            _copyMaterial.SetFloat("_Float", 0.5f);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Property.CurrentHP = 0;
        }
    }

    private void InitializeForestMother()
    {
        _animator = gameObject.GetComponent<Animator>();

        SetMaterial();
    }

    private void SetMaterial()
    {
        _copyMaterial = Instantiate(_material);

        _lowerBody.material = _copyMaterial;

        foreach (var newMaterial in _skinnedMeshRenderer)
        {
            newMaterial.material = _copyMaterial;
        }
    }

    public void TakeDamage(float damage)
    {
        Property.CurrentHP -= damage;

        if (Property.CurrentHP > 0)
        {
            StartCoroutine(IntensityChange(2f, 3f));
        }
    }

    public IEnumerator IntensityChange(float baseValue, float power)
    {
        Color currentColor = _copyMaterial.GetColor("_Color");

        Color intensityUpColor = currentColor * Mathf.Pow(baseValue, power);

        _copyMaterial.SetColor("_Color", intensityUpColor);

        yield return _intensityTime;

        _copyMaterial.SetColor("_Color", currentColor);
    }

    public void Die()
    {
        this.gameObject.layer = LayerMask.NameToLayer("DeadMonster");

        _animator.SetTrigger("Death");

        PlayableDirector motherTimeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.ForestMotherDeath);

        motherTimeLine.Play();

        StartCoroutine(FireShader(5f, 0.5f, -0.3f));
    }

    private IEnumerator FireShader(float maxTime, float startValue, float endValue)
    {
        float elapsedTime = 0f;

        while(elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;

            float colorValue = Mathf.Lerp(startValue, endValue, elapsedTime / maxTime);

            _copyMaterial.SetFloat("_Float", colorValue);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
