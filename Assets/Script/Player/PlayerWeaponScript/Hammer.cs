
public class Hammer : Weapon
{
    public override void SetWeaponData(PlayerWeaponData weaponData, PlayerData playerData)
    {
        _weaponData = weaponData;
        _playerData = playerData;
    }

    public override void UseWeapon(bool isCharge)
    {
        FindTarget(isCharge);
    }
}
