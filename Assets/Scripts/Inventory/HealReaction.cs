﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealReaction : MonoBehaviour
{
    public FloatValue playerHealth;
    public Signal healthSignal;

    public void Use(int amountToIncrease)
    {
        playerHealth.RuntimeValue += amountToIncrease;
        healthSignal.Raise();
    }
}
