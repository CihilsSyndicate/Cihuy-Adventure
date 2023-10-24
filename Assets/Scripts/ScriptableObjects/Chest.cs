using UnityEngine;

[CreateAssetMenu(fileName = "ChestData", menuName = "Chest Data")]
public class Chest : ScriptableObject
{
    public enum ChestType
    {
        SupplyChest,
        WeaponChest
    }

    public ChestType chestType;
    public GameObject[] normalItems;
    public InventoryItem rareItems;
    public string dialog;
    public bool open;
}
