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

        SkillManager.Instance.SkillCount++;

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

        //PlayableDirector motherTimeLine = TimeLineManager.Instance.GetTimeLine(TimeLineType.ForestMother);

        //motherTimeLine.Play();

        gameObject.SetActive(false);
    }
}
