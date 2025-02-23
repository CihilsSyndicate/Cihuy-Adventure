﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    public GameObject shop;
    public Text[] itemNameTxt;
    public Text costTxt;
    public Text hpTxt, atkTxt;
    public Text itemDescTxt;
    public Image[] itemImage;
    private InventoryItem currentItem;
    public Button buyBtn;
    public Text buyBtnText;
    public GameObject coinGO;
    public GameObject[] items;
    public GameObject popupItemDetail;

    // Start is called before the first frame update
    void Start()
    {
        TurnOffBuyBtn();
        for (int i = 0; i < itemNameTxt.Length; i++)
        {
            itemNameTxt[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < itemImage.Length; i++)
        {
            itemImage[i].gameObject.SetActive(false);
        }
        costTxt.gameObject.SetActive(false);
        coinGO.SetActive(false);
    }

    void AddItemToInventory()
    {
        if (playerInventory && currentItem)
        {
            if (playerInventory.myInventory.Contains(currentItem))
            {
                currentItem.numberHeld += 1;
                if (currentItem.itemType == InventoryItem.ItemType.Consumable)
                {
                    currentItem.consumableStack -= 1;
                }
           
            }
            else if(playerInventory.myInventory.Count < playerInventory.maxInventorySize)
            {
                playerInventory.myInventory.Add(currentItem);
                currentItem.numberHeld += 1;
                if (currentItem.itemType == InventoryItem.ItemType.Consumable)
                {
                    currentItem.consumableStack -= 1;
                }
                
            }
            else
            {
                Debug.LogWarning("INVENTORY IS FULL");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem()
    {
        CoinCounter.Instance.DecreaseCoin(currentItem.cost);
        if(currentItem.itemType == InventoryItem.ItemType.Consumable)
        {
            AddItemToInventory();  
            CheckPurchasable();
        }
        else if (currentItem.itemType == InventoryItem.ItemType.Equipment)
        {
            currentItem.isOwned = true;
            AddItemToInventory();
            TurnOffBuyBtn();
        }
    }

    public void GenerateCoin()
    {
        CoinCounter.Instance.IncreaseCoin(100);
    }

    public void DisplayItem(InventoryItem item)
    {
        for (int i = 0; i < itemNameTxt.Length; i++)
        {
            itemNameTxt[i].gameObject.SetActive(true);
        }
        costTxt.gameObject.SetActive(true);
        for (int i = 0; i < itemImage.Length; i++)
        {
            itemImage[i].gameObject.SetActive(true);
        }
        coinGO.SetActive(true);
        popupItemDetail.SetActive(true);
        currentItem = item;

        // Mengisi teks dan gambar berdasarkan item yang dipilih
        for (int i = 0; i < itemNameTxt.Length; i++)
        {
            itemNameTxt[i].text = currentItem.itemName;
        }
        for (int i = 0; i < itemImage.Length; i++)
        {
            itemImage[i].sprite = currentItem.itemImage;
        }
        costTxt.text = currentItem.cost.ToString();
        hpTxt.text = "HP: " + currentItem.Hp.ToString();
        atkTxt.text = "ATK: " + currentItem.Atk.ToString();
        itemDescTxt.text = currentItem.itemDescription;
        CheckPurchasable();
    }

    public void CheckPurchasable()
    {
        if(CoinCounter.Instance.currentCoin >= currentItem.cost && currentItem.consumableStack > 0)
        {
            TurnOnBuyBtn();
        }
        else if (CoinCounter.Instance.currentCoin >= currentItem.cost && currentItem.itemType == InventoryItem.ItemType.Equipment && currentItem.isOwned == false)
        {
            TurnOnBuyBtn();
        }
        else
        {
            TurnOffBuyBtn();
        }
    }

    private void TurnOnBuyBtn()
    {
        buyBtn.interactable = true;
        Color textCol = buyBtnText.color;
        textCol.a = 1f;
        buyBtnText.color = textCol;
    }

    private void TurnOffBuyBtn()
    {
        buyBtn.interactable = false;
        Color textCol = buyBtnText.color;
        textCol.a = 0.5f;
        buyBtnText.color = textCol;
    }

    public void TurnOnInteractButton()
    {
        PlayerMovement.Instance.interactButton.interactable = true;
        PlayerMovement.Instance.npcSign.dialogActive = false;
    }
}
