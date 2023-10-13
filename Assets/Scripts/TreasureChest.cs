using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public Chest chestData;
    public GameObject[] sign;
    public Animator anim;
    public bool open;

    private void Start()
    {
        open = false;
        for (int i = 0; i < sign.Length; i++)
        {
            if (sign[i].activeInHierarchy)
            {
                sign[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger && !open)
        {
            sign[0].SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            sign[0].SetActive(false);
        }
    }

    public void OpenChest()
    {
        anim.SetBool("open", true);
        PlayerMovement.Instance.anim.SetBool("Celebration", true);
        open = true;
        sign[0].SetActive(false);

        if (chestData.chestType == Chest.ChestType.Rare)
        {
            // Tambahkan item ke PlayerInventory
            AddToPlayerInventory(chestData.rareItems);
        }
        else if (chestData.chestType == Chest.ChestType.Normal)
        {
            // Generate GameObject jika tipe chest Normal
            foreach (GameObject itemPrefab in chestData.normalItems)
            {
                GameObject item = Instantiate(itemPrefab);
                item.transform.SetParent(this.gameObject.transform);
                item.transform.position = transform.position;
            }
        }
    }

    private void AddToPlayerInventory(InventoryItem[] items)
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerInventory != null)
        {
            foreach (InventoryItem item in items)
            {
                playerInventory.myInventory.Add(item);
            }
        }
        else
        {
            Debug.LogWarning("PlayerInventory not found.");
        }
    }
}
