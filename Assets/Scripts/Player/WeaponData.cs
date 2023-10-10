using System;
[Serializable]
public class WeaponData
{
    public InventoryItem currentItem;

    public WeaponData(WeaponManager weaponManager)
    {
        currentItem = weaponManager.weapon;
    }
}
