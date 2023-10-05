﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public GameObject[] go;
    public bool trader;
    private static NPCInteract instance;

    public static NPCInteract Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    public void Interact()
    {
        if (trader == true)
        {
            go[0].SetActive(true);
            go[1].SetActive(false);
        }
        else if(trader == false)
        {
            go[2].SetActive(false);
        }
    }
}