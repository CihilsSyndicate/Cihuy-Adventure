using UnityEngine;

[System.Serializable]
public class ChestData
{
    public bool open;
}

[CreateAssetMenu(fileName = "ChestData", menuName = "Chest Data")]
public class Chest : ScriptableObject
{
    public enum ChestType
    {
        Normal,
        Rare
    }

    public ChestData chestData;
    public ChestType chestType;
    public GameObject[] normalItems;
    public InventoryItem rareItems;
    public string dialog;
}
