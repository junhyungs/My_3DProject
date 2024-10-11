using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponManager : Singleton<WeaponManager>
{
    private PlayerWeaponController _weaponController;

    public void ChangeWeapon(PlayerWeapon weapon)
    {
        _weaponController.SetWeapon(weapon);
    }

    public void SetWeaponController(PlayerWeaponController weaponController)
    {
        _weaponController = weaponController;
    }

    public IEnumerator LoadWeaponData(string Id, Weapon weaponComponent, PlayerWeaponEffectController effectController)
    {
        yield return new WaitWhile(() =>
        {
            Debug.Log("���� �����͸� �������� ���߽��ϴ�");
            return DataManager.Instance.GetData(Id) == null;
        });

        var weaponData = DataManager.Instance.GetData(Id) as PlayerWeaponData;
        
        effectController.SetEffectRange(weaponData.NormalEffectRange, weaponData.ChargeEffectRange);

        weaponComponent.SetWeaponData(weaponData);
    }
}
