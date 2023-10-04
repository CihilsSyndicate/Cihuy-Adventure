using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public GameObject[] sign;
    public Animator anim;
    public Collider2D[] col;
    public GameObject healPotionPrefabs;

    private void Start()
    {
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
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            sign[0].SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            sign[0].SetActive(false);
        }
    }

    public void OpenChest()
    {
        anim.SetBool("open", true);
        col[0].enabled = false;
        col[1].enabled = false;
        for (int i = 0; i < 2; i++)
        {
            GameObject healPotion = Instantiate(healPotionPrefabs);
            healPotion.transform.SetParent(this.gameObject.transform);
            healPotion.transform.position = transform.position;
        }
    }
}
