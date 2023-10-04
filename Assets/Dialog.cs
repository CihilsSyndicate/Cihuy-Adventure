using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public bool shop;
    public GameObject[] go;
    

    public void InteractDialog()
    {       

        if (shop)
        {
            go[0].SetActive(true);
        }
        else
        {
            go[1].SetActive(false);
        }     
    }
}
