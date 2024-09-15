using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWeapon
{
    Sword,
    Hammer,
    Dagger,
    GreatSword,
    Umbrella
}

public class WeaponManager : Singleton<WeaponManager>
{    
    private PlayerWeaponEffectController _effectController;
    private PlayerWeaponController _weaponController;
    private PlayerWeapon _currentWeapon;

    private void Awake()
    {
        _effectController = gameObject.GetComponent<PlayerWeaponEffectController>();
    }

    public void SetCurrentWeapon(PlayerWeapon weapon)
    {
        _currentWeapon = weapon;
    }

    public void SetWeaponController(PlayerWeaponController weaponController)
    {
        _weaponController = weaponController;
    }

    public IEnumerator LoadWeaponData(string Id, Weapon weaponComponent)
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("무기 데이터를 가져오지 못했습니다");
            return DataManager.Instance.GetData(Id) == null;
        });

        var weaponData = DataManager.Instance.GetData(Id) as PlayerWeaponData;

        _effectController.SetEffectRange(weaponData.NormalEffectRange, weaponData.ChargeEffectRange);

        weaponComponent.SetWeaponData(weaponData);
    }
}
