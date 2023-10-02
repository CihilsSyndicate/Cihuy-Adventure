using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Stuff to change")]
    //[SerializeField] private Text itemNumberText;
    [SerializeField] private Image itemImage;

    [Header("Variables from the item")]
    public Sprite itemSprite;
   // public int numberHeld;
    public string itemDescription;
    public float Atk;
    public float Hp;
    public InventoryItem thisItem;
    public InventoryManager thisManager;

    public void Setup(InventoryItem newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem != null)
        {
            itemImage.sprite = thisItem.itemImage;
            //itemNumberText.text = thisItem.numberHeld.ToString();
        }
    }

    public void ClickedOn()
    {
        if (thisItem)
        {
            thisManager.SetupDescriptionAndButton(thisItem.itemDescription, thisItem.usable, "Hp : +" + thisItem.Hp.ToString(), "Atk : +" + thisItem.Atk.ToString(), thisItem.itemName, thisItem.itemImage);
        }
    }
}
