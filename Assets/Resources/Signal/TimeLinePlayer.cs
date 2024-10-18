using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLinePlayer : MonoBehaviour
{
    private enum DummyPlayerWeapon
    {
        Sword,
        Hammer,
        Dagger,
        GreatSword,
        Umbrella
    }

    [Header("DummyWeapon")]
    [SerializeField] private GameObject[] _weapons;
    private Animator _animator;

    private Dictionary<DummyPlayerWeapon, GameObject> _weaponDictionary = new Dictionary<DummyPlayerWeapon, GameObject>();

    private void Awake()
    {
        for(int i = 0; i < _weapons.Length; i++)
        {
            _weaponDictionary.Add((DummyPlayerWeapon)i, _weapons[i]);
        }
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _animator.applyRootMotion = true;

        var currentWeapon = WeaponManager.Instance.CurrentWeapon;

        switch(currentWeapon)
        {
            case PlayerWeapon.Sword:
                GameObject sword = _weaponDictionary[DummyPlayerWeapon.Sword];
                sword.SetActive(true);
                break;
            case PlayerWeapon.Hammer:
                GameObject hammer = _weaponDictionary[DummyPlayerWeapon.Hammer];
                hammer.SetActive(true);
                break;
            case PlayerWeapon.Dagger:
                GameObject dagger = _weaponDictionary[DummyPlayerWeapon.Dagger];
                dagger.SetActive(true);
                break;
            case PlayerWeapon.GreatSword:
                GameObject greatSword = _weaponDictionary[DummyPlayerWeapon.GreatSword];
                greatSword.SetActive(true);
                break;
            case PlayerWeapon.Umbrella:
                GameObject umbrella = _weaponDictionary[DummyPlayerWeapon.Umbrella];
                umbrella.SetActive(true);
                break;
        }
    }
}
