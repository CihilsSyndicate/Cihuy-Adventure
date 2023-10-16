﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public Chest chestData;
    public NpcSign npcSign;
    public SpriteRenderer itemGainedSR;
    public PlayerInventory playerInventory;
    public InventoryItem mommyKey;
    public GameObject sign;
    public Animator anim;
    public bool needKey;
    public bool needMommyKey;

    private void Start()
    {
        
        if (chestData.chestType == Chest.ChestType.Rare)
        {
            itemGainedSR = GameObject.Find("ItemGained").GetComponent<SpriteRenderer>();
        }
        if (sign.activeInHierarchy)
        {
            sign.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Chest_" + GetInstanceID()) == 1)
        {
            anim.SetBool("open", true);
            chestData.open = true;
        }
        else
            chestData.open = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger && !chestData.open)
        {
            sign.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            sign.SetActive(false);
        }
    }

    public void OpenChest()
    {       
        if(needMommyKey && !playerInventory.myInventory.Contains(mommyKey))
        {
            Debug.LogWarning("You don't have the key");
            return;
        }

        chestData.open = true;
        PlayerPrefs.SetInt("Chest_" + GetInstanceID(), 1);
        sign.SetActive(false);
        anim.SetBool("open", true);
        if (chestData.chestType == Chest.ChestType.Rare)
        {
            PlayerMovement.Instance.anim.SetBool("Celebration", true);    
            playerInventory.myInventory.Add(chestData.rareItems);
            itemGainedSR.sprite = chestData.rareItems.itemImage;
            npcSign.BeginDialog();
            itemGainedSR.enabled = true;
            playerInventory.myInventory.Remove(mommyKey);
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
}
