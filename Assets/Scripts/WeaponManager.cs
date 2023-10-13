﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public InventoryItem defaultWeapon;
    public SpriteRenderer spriteWeapon;
    public Slash slash;
    private static WeaponManager instance;
    public List<InventoryItem> availableWeapons;
    public static WeaponManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
        EquipDefaultWeapon();
    }

    public void SaveEquippedWeapon()
    {
        PlayerPrefs.SetString("EquippedWeaponName", defaultWeapon.itemName);

        PlayerPrefs.Save();
    }

    public void LoadEquippedWeapon()
    {
        if (PlayerPrefs.HasKey("EquippedWeaponName"))
        {
            string weaponName = PlayerPrefs.GetString("EquippedWeaponName");

            InventoryItem loadedWeapon = availableWeapons.Find(weapon => weapon.itemName == weaponName);

            if (loadedWeapon != null)
            {
                defaultWeapon = loadedWeapon;
                EquipWeapon();
            }
            else
            {
               
            }
        }
    }

    public void EquipWeapon()
    {
        spriteWeapon.sprite = defaultWeapon.itemImage;
        slash.damage = defaultWeapon.Atk;
        SwordAttack.Instance.ClearSlashPool();
        SwordAttack.Instance.InitializePool();
        SaveEquippedWeapon();
    }

    public void EquipDefaultWeapon()
    {
        spriteWeapon.sprite = defaultWeapon.itemImage;
        slash.damage = defaultWeapon.Atk;
    }
}
