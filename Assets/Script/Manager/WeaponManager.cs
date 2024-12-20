using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponManager : Singleton<WeaponManager>
{
    private PlayerWeaponController _weaponController;
    public PlayerWeapon CurrentWeapon { get; set; }

    public void ChangeWeapon(PlayerWeapon weapon)
    {
        CurrentWeapon = weapon;

        _weaponController.SetWeapon(weapon);
    }

    public void SetWeaponController(PlayerWeaponController weaponController)
    {
        _weaponController = weaponController;
    }

    public IEnumerator LoadWeaponData(string Id, IWeapon weaponComponent, PlayerData data)
    {
        yield return new WaitWhile(() =>
        {
            return DataManager.Instance.GetData(Id) == null;
        });

        Debug.Log("���� �����͸� �����Խ��ϴ�.");
        var weaponData = DataManager.Instance.GetData(Id) as PlayerWeaponData;

        weaponComponent.SetWeaponData(weaponData, data);
    }
}
