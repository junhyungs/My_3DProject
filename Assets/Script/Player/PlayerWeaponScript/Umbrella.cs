
public class Umbrella : Weapon
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
}
