using UnityEngine;
using UnityEngine.UI;

public class UsePotion : MonoBehaviour
{
    public Button usePotionButton;
    public InventoryItem potionItem; // Item potion yang akan digunakan
    public Image buttonImage; // Komponen Image pada tombol
    public PlayerInventory playerInventory;

    private void Start()
    {
        if (potionItem != null && playerInventory.myInventory.Contains(potionItem))
        {
            buttonImage.sprite = potionItem.itemImage;
        }
    }

    private void Update()
    {
        if (potionItem.numberHeld > 0)
        {
            usePotionButton.interactable = true;
            buttonImage.color = Color.white;
        }
        else
        {
            playerInventory.RemoveItem(potionItem);
            usePotionButton.interactable = false;
            Color newColor = buttonImage.color;
            newColor.a = 0;
            buttonImage.color = newColor;
        }
    }

    public void UseThePotion()
    {
        if (potionItem != null && potionItem.numberHeld > 0 && PlayerMovement.Instance.currentHealth.RuntimeValue < PlayerMovement.Instance.currentHealth.initialValue)
        {
            potionItem.Use();
        }
        else
        {
            PlayerMovement.Instance.ShowFloatingTextHp();
        }
    }
}
