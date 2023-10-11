using System.Collections;
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
        EquipWeapon();
    }

    public void SaveEquippedWeapon()
    {
        // Simpan informasi tentang senjata yang sedang dipakai
        PlayerPrefs.SetString("EquippedWeaponName", defaultWeapon.itemName);

        PlayerPrefs.Save();
    }

    public void LoadEquippedWeapon()
    {
        // Memulihkan senjata yang terakhir kali digunakan
        if (PlayerPrefs.HasKey("EquippedWeaponName"))
        {
            string weaponName = PlayerPrefs.GetString("EquippedWeaponName");

            // Cari senjata dengan nama yang sesuai dalam daftar availableWeapons
            InventoryItem loadedWeapon = availableWeapons.Find(weapon => weapon.itemName == weaponName);

            if (loadedWeapon != null)
            {
                defaultWeapon = loadedWeapon;
                EquipWeapon();
            }
            else
            {
                Debug.LogWarning("Senjata dengan nama yang disimpan tidak ditemukan dalam daftar availableWeapons.");
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
}
