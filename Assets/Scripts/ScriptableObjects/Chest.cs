using UnityEngine;

[CreateAssetMenu(fileName = "ChestData", menuName = "Chest Data")]
public class Chest : ScriptableObject
{
    public enum ChestType
    {
        Normal,
        Rare
    }

    public ChestType chestType;
    public GameObject[] normalItems;
    public InventoryItem[] rareItems;

}
