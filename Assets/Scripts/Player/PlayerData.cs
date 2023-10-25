﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public float health;


    public PlayerData (PlayerMovement player)
    {
        health = player.currentHealth.RuntimeValue;
    }
}
