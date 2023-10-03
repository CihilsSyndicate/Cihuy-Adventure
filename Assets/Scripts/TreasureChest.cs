using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public GameObject[] sign;
    public Animator anim;

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
}
