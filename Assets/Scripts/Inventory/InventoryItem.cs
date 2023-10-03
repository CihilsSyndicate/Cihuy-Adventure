using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
[System.Serializable]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int cost;
    public float Atk;
    public float Hp;
    public Sprite itemImage;
    public int numberHeld;
    public int consumableStack;
    public bool usable;
    public bool unique;
    public bool isOwned;
    public UnityEvent thisEvent;
    public ItemType itemType;
    public enum ItemType
    {
        Equipment,
        Consumable
    }

    public void Use()
    {
        thisEvent.Invoke();
    }

    public void DecraeseAmount(int amountToDecrease)
    {
        if(itemType == ItemType.Consumable)
        {
            numberHeld -= amountToDecrease;
            if (numberHeld < 0)
            {
                numberHeld = 0;
            }
        }
      
    }

}
