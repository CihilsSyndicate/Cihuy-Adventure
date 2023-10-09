using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    public int maxInventorySize = 20;
    public List<InventoryItem> myInventory = new List<InventoryItem>();

    public bool AddItem(InventoryItem item, GameObject go, InventoryItem currentItem)
    {
        if (myInventory.Count < maxInventorySize)
        {
            myInventory.Add(item);
            currentItem.numberHeld += 1;
            Destroy(go);
            return true;
            
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        myInventory.Remove(item);
    }
}
