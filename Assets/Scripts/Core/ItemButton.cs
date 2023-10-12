using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public InventoryItem item; // Item yang akan ditampilkan saat tombol ini diklik
    private Text consumableStackTxt;
    public Image image;
    private ShopManager shopManager;

    private void Start()
    {
        consumableStackTxt = GetComponentInChildren<Text>();
        if(item.itemType == InventoryItem.ItemType.Consumable)
        {
            item.consumableStack = Random.Range(3, 5);
        }
        else
        {
            consumableStackTxt.gameObject.SetActive(false);
        }
        image.sprite = item.itemImage;
        shopManager = FindObjectOfType<ShopManager>();
    }

    private void Update()
    {
        if(item.itemType == InventoryItem.ItemType.Consumable)
            consumableStackTxt.text = item.consumableStack.ToString();
    }

    public void OnItemClick()
    {
        shopManager.DisplayItem(item);
    }
}
