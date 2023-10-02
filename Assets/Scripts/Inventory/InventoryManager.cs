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

    public void SetTextAndButton(string description, bool buttonActive)
    {
        descriptionText.text = description;
        if (buttonActive)
        {
            useButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
        }
    }

    void MakeInventorySlot()
    {
        if (playerInventory != null)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                GameObject temp =
                    Instantiate(blackInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                temp.transform.SetParent(inventoryPanel.transform);
                InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                if(newSlot != null)
                {
                    newSlot.Setup(playerInventory.myInventory[i], this);
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

    // Start is called before the first frame update
    void Start()
    {
        MakeInventorySlot();
        SetTextAndButton("", false);
    }

    public void SetupDescriptionAndButton(string newDescriptionString, bool isButtonUsable, string newHPText, string newATKText, string newItemName, Sprite newItemImage)
    {
        descriptionText.text = newDescriptionString;
        atkText.text = newATKText;
        hpText.text = newHPText;
        itemNameText.text = newItemName;
        itemImage.sprite = newItemImage;
        useButton.SetActive(isButtonUsable);
    }

}
