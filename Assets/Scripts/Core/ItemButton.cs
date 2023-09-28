using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public ShopItems item; // Item yang akan ditampilkan saat tombol ini diklik
    private Text consumableStackTxt;
    private Image image;
    private ShopManager shopManager;

    private void Start()
    {
        consumableStackTxt = GetComponentInChildren<Text>();
        if(item.itemType == ShopItems.ItemType.Consumable)
        {
            item.consumableStack = Random.Range(3, 5);
        }
        else
        {
            consumableStackTxt.gameObject.SetActive(false);
        }
        image = GetComponent<Image>();
        image.sprite = item.itemSprite;
        shopManager = FindObjectOfType<ShopManager>();
    }

    private void Update()
    {
        if(item.itemType == ShopItems.ItemType.Consumable)
            consumableStackTxt.text = item.consumableStack.ToString();
    }

    public void OnItemClick()
    {
        shopManager.DisplayItem(item);
    }
}
