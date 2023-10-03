using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    [SerializeField] private GameObject blackInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text atkText;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject detailButton;
    [SerializeField] private GameObject dropButton;
    public InventoryItem currentItem;
    public FloatValue playerHealth;

    public void SetTextAndButton(string description, bool buttonActive)
    {
        descriptionText.text = description;
        if (buttonActive)
        {
            useButton.SetActive(true);
            detailButton.SetActive(true);
            dropButton.SetActive(true);
        }
        else
        {         
            useButton.SetActive(false);
            detailButton.SetActive(false);
            dropButton.SetActive(false);
        }
    }

    void MakeInventorySlot()
    {
        if (playerInventory != null)
        {
            
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {        
                if(playerInventory .myInventory[i].numberHeld > 0)
                {
                    GameObject temp =
                   Instantiate(blackInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                    temp.transform.SetParent(inventoryPanel.transform);
                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    if (newSlot != null)
                    {
                        newSlot.Setup(playerInventory.myInventory[i], this);
                    }
                }          
            }
        }
    }

    public void OpenPOPUP(GameObject go)
    {
        go.SetActive(true);
    }

    public void ClosePOPUP(GameObject go)
    {
        go.SetActive(false);
    }

    public void RemoveItemsWithZeroCount()
    {
        List<InventoryItem> itemsToRemove = new List<InventoryItem>();

        for (int i = 0; i < playerInventory.myInventory.Count; i++)
        {
            if (playerInventory.myInventory[i].numberHeld == 0)
            {
                itemsToRemove.Add(playerInventory.myInventory[i]);
            }
        }

        // Hapus objek yang sesuai dengan kondisi di atas dari List playerInventory.myInventory
        foreach (var item in itemsToRemove)
        {
            playerInventory.myInventory.Remove(item);
        }
    }


    // Start is called before the first frame update
    void OnEnable()
    {   
        ClearInventoryItem();
        MakeInventorySlot();
        SetTextAndButton("", false);
    }

    void ClearInventoryItem()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }

    public void SetupDescriptionAndButtonForConsumable(string newDescriptionString, bool isButtonUsable, string newHPText, string newATKText, string newItemName, Sprite newItemImage, InventoryItem newItem)
    {
        currentItem = newItem;
        descriptionText.text = newDescriptionString;
        atkText.text = newATKText;
        hpText.text = newHPText;
        itemNameText.text = newItemName;
        itemImage.sprite = newItemImage;
        useButton.SetActive(isButtonUsable);
        detailButton.SetActive(isButtonUsable);
        dropButton.SetActive(isButtonUsable);
    }

    public void UseButtonPressed()
    {
        if (currentItem && currentItem.itemType != InventoryItem.ItemType.Equipment)
        {
            if(playerHealth.RuntimeValue != playerHealth.initialValue)
            {
                currentItem.Use();
                if(currentItem.numberHeld == 0)
                {
                    RemoveItemsWithZeroCount();
                    SetTextAndButton("", false);
                }
                ClearInventoryItem();
                MakeInventorySlot();
            }
            
        }
    }

}
