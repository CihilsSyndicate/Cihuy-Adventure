using UnityEngine;
using UnityEngine.UI;

public class UsePotion : MonoBehaviour
{
    public InventoryItem potionItem; // Item potion yang akan digunakan
    public Image buttonImage; // Komponen Image pada tombol
    public Text quantityText; // Komponen Text (opsional) untuk menampilkan jumlah potion yang tersisa

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>(); // Cari objek PlayerInventory di scene

        // Set gambar dan teks tombol sesuai dengan item potion
        if (potionItem != null)
        {
            buttonImage.sprite = potionItem.itemImage;
            UpdateButtonState();
        }
    }

    private void Update()
    {

    }

    // Method yang dipanggil saat tombol diklik
    public void UseThePotion()
    {
        if (potionItem != null && potionItem.numberHeld > 0 && PlayerMovement.Instance.currentHealth.RuntimeValue < PlayerMovement.Instance.currentHealth.initialValue)
        {
            // Gunakan potion
            potionItem.Use();

            // Perbarui tampilan tombol
            UpdateButtonState();
        }
    }

    // Metode untuk mengaktifkan atau menonaktifkan tombol berdasarkan jumlah potion yang tersisa
    private void UpdateButtonState()
    {
        if (potionItem.numberHeld > 0)
        {
            buttonImage.color = Color.white;
            quantityText.text = "x" + potionItem.numberHeld.ToString();
        }
        else
        {
            buttonImage.gameObject.SetActive(false);
            quantityText.gameObject.SetActive(false);
        }
    }
}
