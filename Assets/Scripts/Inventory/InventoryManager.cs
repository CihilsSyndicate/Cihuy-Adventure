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
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject detailButton;
    [SerializeField] private GameObject dropButton;

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

    // Start is called before the first frame update
    void Start()
    {
        MakeInventorySlot();
        SetTextAndButton("", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
