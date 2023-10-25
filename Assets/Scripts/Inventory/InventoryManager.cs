using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Text deductionText;
    public int deduction;

    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    public GameObject blackInventorySlot;
    public GameObject inventoryPanel;
    public Text descriptionText;
    public Text hpText;
    public Text atkText;
    public Text itemNameText;
    public Image itemImage;
    public Image reduceItemImage;
    public GameObject useButton, equipButton, detailButton, dropButton, reduceButton;
    public InventoryItem currentItem;
    public FloatValue playerHealth;

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
                        newSlot.transform.localScale = blackInventorySlot.transform.localScale;
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
        descriptionText.text = "";
        useButton.SetActive(false);
        equipButton.SetActive(false);
        detailButton.SetActive(false);
        dropButton.SetActive(false);
        reduceButton.SetActive(false);
    }

    void ClearInventoryItem()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }

    public void RemoveItem(GameObject GO)
    {
        GO.SetActive(false);
        WeaponManager.Instance.RemoveAndUseWeapon(currentItem);
        ClearInventoryItem();
        MakeInventorySlot();
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
                    descriptionText.text = "";
                    useButton.SetActive(false);
                    equipButton.SetActive(false);
                    detailButton.SetActive(false);
                    dropButton.SetActive(false);
                }
                ClearInventoryItem();
                MakeInventorySlot();
            } 
        }
    }

    public void OpenPopupReduce(GameObject go)
    {
        go.SetActive(true);
        reduceItemImage.sprite = currentItem.itemImage;
    }

    public void ReduceItem(GameObject go)
    {
        if (deduction != 0)
        {
            go.SetActive(false);
            currentItem.numberHeld -= deduction;
            deduction = 0;
            deductionText.text = deduction.ToString();
            ClearInventoryItem();
            MakeInventorySlot();
        }
    }

    public void ClosePopupReduce(GameObject go)
    {
        go.SetActive(false);
        deduction = 0;
        deductionText.text = deduction.ToString();
    }

    public void IncreaseDeduction()
    {
        if(deduction < currentItem.numberHeld)
        {
            deduction += 1;
            deductionText.text = deduction.ToString();
        }
    }

    public void DecreaseDeduction()
    {
        if(deduction > 0)
        {
            deduction -= 1;
            deductionText.text = deduction.ToString();
        }
    }
}
