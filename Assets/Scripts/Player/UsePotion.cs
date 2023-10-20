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

    }

    private void Update()
    {
        if (playerInventory.myInventory.Contains(potionItem))
        {
            if (potionItem.numberHeld > 0)
            {
                usePotionButton.interactable = true;
                buttonImage.enabled = true;
            }
            else
            {
                playerInventory.RemoveItem(potionItem);
            }
        }
        else
        {
            buttonImage.enabled = false;
            usePotionButton.interactable = false;
        }
    }

    public void UseThePotion()
    {
        if (potionItem != null && potionItem.numberHeld > 0 && PlayerMovement.Instance.currentHealth.RuntimeValue < PlayerMovement.Instance.currentHealth.initialValue)
        {
            potionItem.Use();
            if(potionItem.numberHeld == 0)
            {
                playerInventory.myInventory.Remove(potionItem);
            }
        }
        else
        {
            PlayerMovement.Instance.ShowFloatingTextHp();
        }
    }
}
