using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shop;
    public Text itemNameTxt;
    public Text costTxt;
    public Image itemImage;
    private ShopItems currentItem;
    public Button buyBtn;
    public Text buyBtnText;
    public GameObject coinGO;
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        itemNameTxt.gameObject.SetActive(false);
        costTxt.gameObject.SetActive(false);
        itemImage.gameObject.SetActive(false);
        TurnOffBuyBtn();
        coinGO.SetActive(false);

        foreach (var item in items)
        {
            ShopItems shopItem = item.GetComponent<ItemButton>().item;
            if (shopItem != null && shopItem.isOwned)
            {
                item.SetActive(false);
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
        if(currentItem.itemType == ShopItems.ItemType.Consumable)
        {
            currentItem.consumableStack -= 1;
            currentItem.quantity += 1;
            Debug.Log("Kuantitas: " + currentItem.quantity);
            if(currentItem.consumableStack <= 0)
            {
                TurnOffBuyBtn();
            }
        }
        else if (currentItem.itemType == ShopItems.ItemType.Equipment)
        {
            currentItem.isOwned = true;
            TurnOffBuyBtn();
        }
    }

    public void GenerateCoin()
    {
        CoinCounter.Instance.IncreaseCoin(100);
    }

    public void DisplayItem(ShopItems item)
    {
        itemNameTxt.gameObject.SetActive(true);
        costTxt.gameObject.SetActive(true);
        itemImage.gameObject.SetActive(true);
        coinGO.SetActive(true);
        TurnOnBuyBtn();

        currentItem = item;

        // Mengisi teks dan gambar berdasarkan item yang dipilih
        itemNameTxt.text = currentItem.itemName;
        costTxt.text = currentItem.cost.ToString();
        itemImage.sprite = currentItem.itemSprite;
        CheckPurchasable();
    }

    public void CheckPurchasable()
    {
        if(CoinCounter.Instance.currentCoin >= currentItem.cost && currentItem.isOwned == false && currentItem.consumableStack > 0)
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
}
