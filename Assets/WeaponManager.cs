using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public InventoryItem weapon;
    public SpriteRenderer spriteWeapon;
    private static WeaponManager instance;
    public static WeaponManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    public void EquipWeapon()
    {
        spriteWeapon.sprite = weapon.itemImage;
    }
}
