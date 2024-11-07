using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponEffectController : MonoBehaviour
{
    private enum Range
    {
        Normal,
        Charge
    }

    [Header("Transform")]
    [SerializeField] private Transform _effectTransform;

    private Dictionary<PlayerWeapon, Color> _colorValueDictionary = new Dictionary<PlayerWeapon, Color>();
    private Dictionary<PlayerWeapon, List<Vector3>> _rangeDictionary = new Dictionary<PlayerWeapon, List<Vector3>>();

    private GameObject _effectObject;
    private ParticleSystem _effectPaticleSystem;
    public Material _effectMaterial;

    private void Awake()
    {
        InitializeOnAwake();
    }

    private void Start()
    {
        InitializeOnStart();
    }

    private void InitializeOnAwake()
    {
        InitializeMaterial();

        InitializeColorDictionary();

        InitializeEffectObject();
    }

    private void InitializeOnStart()
    {
        StartCoroutine(InitializeRangeDictionary());
    }

    private void InitializeEffectObject()
    {
        GameObject loadObject = Resources.Load<GameObject>("Prefab/Player/Slash");

        GameObject effectObject = Instantiate(loadObject, _effectTransform);

        effectObject.transform.localPosition = Vector3.zero; 

        _effectPaticleSystem = effectObject.GetComponent<ParticleSystem>();

        _effectObject = effectObject;
    }

    private void InitializeMaterial()
    {
        Material material = Resources.Load<Material>("Prefab/Player/FX/SwordSlash");

        Color color = GetWeaponColor(PlayerWeapon.Sword);

        material.SetColor("_AddColor", color);

        _effectMaterial = material;
    }

    private void InitializeColorDictionary()
    {
        _colorValueDictionary.Add(PlayerWeapon.Sword, new Color(1f, 13f / 255f, 8f / 255f));
        _colorValueDictionary.Add(PlayerWeapon.Hammer, new Color(0f, 1f, 1f));
        _colorValueDictionary.Add(PlayerWeapon.Dagger, new Color(38f / 255f, 1f, 18f / 255f));
        _colorValueDictionary.Add(PlayerWeapon.GreatSword, new Color(1f, 0f, 228f / 255f));
        _colorValueDictionary.Add(PlayerWeapon.Umbrella, new Color(0f, 4f / 255f, 1f));
    }

    private IEnumerator InitializeRangeDictionary()
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData("W101") == null;
        });

        for(int i = 0; i < 5; i++)
        {
            var data = DataManager.Instance.GetData($"W10{i+1}") as PlayerWeaponData;

            float normalRange = data.NormalEffectRange;

            float chargeRange = data.ChargeEffectRange;

            List<Vector3> rangeList = new List<Vector3>
            {
                new Vector3(normalRange, normalRange, normalRange),
                new Vector3(chargeRange, chargeRange, chargeRange)
            };

            _rangeDictionary.Add((PlayerWeapon)i, rangeList);
        }
    }

    private Color GetWeaponColor(PlayerWeapon weapon)
    {
        if (_colorValueDictionary.ContainsKey(weapon))
        {
            return _colorValueDictionary[weapon];
        }

        return Color.white;
    }

    private Vector3 GetEffectRange(bool isCharge ,PlayerWeapon currentWeapon)
    {
        if (_rangeDictionary.ContainsKey(currentWeapon))
        {
            List<Vector3> rangeList = _rangeDictionary[currentWeapon];

            Vector3 range = isCharge ? rangeList[(int)Range.Charge] : rangeList[(int)Range.Normal];

            return range;
        }

        Debug.Log("정상적인 이펙트 범위를 리턴하지 못했습니다.");
        return Vector3.zero;
    }

    public void ChageColor(PlayerWeapon weapon)
    {
        Color newColor = GetWeaponColor(weapon);

        _effectMaterial.SetColor("_AddColor", newColor);
    }

    public void ActiveSwordEffect_L(bool isCharge, PlayerWeapon currentWeapon)
    {
        Vector3 effectRange = GetEffectRange(isCharge, currentWeapon);

        _effectObject.transform.localScale = effectRange;
        _effectObject.transform.localRotation = Quaternion.identity;
        _effectPaticleSystem.Play();
    }

    public void ActiveSwordEffect_R(bool isCharge, PlayerWeapon currentWeapon)
    {
        Vector3 effectRange = GetEffectRange(isCharge, currentWeapon);

        _effectObject.transform.localScale = effectRange;
        _effectObject.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        _effectPaticleSystem.Play();
    }
}
