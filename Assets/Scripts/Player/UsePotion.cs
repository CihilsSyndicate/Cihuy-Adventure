using UnityEngine;
using UnityEngine.UI;

public class UsePotion : MonoBehaviour
{
    public InventoryItem potionItem; // Item potion yang akan digunakan
    public Image buttonImage; // Komponen Image pada tombol
    public Text quantityText; // Komponen Text (opsional) untuk menampilkan jumlah potion yang tersisa

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
        quantityText.text = "x" + potionItem.numberHeld.ToString();
        if (potionItem.numberHeld > 0)
        {
            buttonImage.color = Color.white;
        }
        else
        {
            buttonImage.color = Color.gray;
        }
    }

    public void UseThePotion()
    {
        if (potionItem != null && potionItem.numberHeld > 0 && PlayerMovement.Instance.currentHealth.RuntimeValue < PlayerMovement.Instance.currentHealth.initialValue)
        {
            potionItem.Use();
        }
    }
}
