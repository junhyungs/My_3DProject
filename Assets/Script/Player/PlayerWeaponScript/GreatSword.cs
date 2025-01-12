
using UnityEngine;

public class GreatSword : Weapon
{
    public override void SetWeaponData(PlayerWeaponData weaponData, PlayerData playerData)
    {
        _playerData = playerData;
        _weaponData = weaponData;
    }

    public override void UseWeapon(bool isCharge)
    {
        FindTarget(isCharge);
    }

    public void OnDrawGizmos()
    {
        Vector3 boxSize = _weaponData.NormalAttackRange;

        DrawGizmos(boxSize);
    }
}
