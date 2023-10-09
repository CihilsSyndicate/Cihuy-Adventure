using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Stuff to change")]
    [SerializeField] private Text itemNumberText;
    [SerializeField] private Image itemImage;

    [Header("Variables from the item")]
    public Sprite itemSprite;
    public int numberHeld;
    public string itemDescription;
    public float Atk;
    public float Hp;
    public InventoryItem thisItem;
    public InventoryManager thisManager;

    private void Start()
    {
        if(thisItem.itemType == InventoryItem.ItemType.Consumable)
        {
            itemNumberText.text = thisItem.numberHeld.ToString();
        }else
        {
            itemNumberText.gameObject.SetActive(false);
        }
       
    }

    public void Setup(InventoryItem newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem)
        {
            itemImage.sprite = thisItem.itemImage; 
        }
    }

    public void ClickedOn()
    {
        thisManager.descriptionText.text = thisItem.itemDescription;
        thisManager.atkText.text = "ATK: " + thisItem.Atk.ToString();
        thisManager.hpText.text = "HP: " + thisItem.Hp.ToString();
        thisManager.itemNameText.text = thisItem.itemName;
        thisManager.itemImage.sprite = thisItem.itemImage;
        thisManager.detailButton.SetActive(true);
        thisManager.dropButton.SetActive(true);
        if (thisItem.itemType == InventoryItem.ItemType.Consumable)
        {
            thisManager.useButton.SetActive(true);
            thisManager.equipButton.SetActive(false);
        }
        else if(thisItem.itemType == InventoryItem.ItemType.Equipment)
        {
            thisManager.useButton.SetActive(false);
            thisManager.equipButton.SetActive(true);
        }
    }
}
