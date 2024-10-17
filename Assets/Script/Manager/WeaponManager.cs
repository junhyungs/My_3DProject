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

    public IEnumerator LoadWeaponData(string Id, Weapon weaponComponent)
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("���� �����͸� �������� ���߽��ϴ�");
            return DataManager.Instance.GetData(Id) == null;
        });

        var weaponData = DataManager.Instance.GetData(Id) as PlayerWeaponData;

        weaponComponent.SetWeaponData(weaponData);
    }
}
