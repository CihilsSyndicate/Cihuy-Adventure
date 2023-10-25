using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{  
    public Slider slider;

    public void SetMaxHealth(FloatValue health)
    {
        slider.maxValue = health.initialValue;
        slider.value = health.initialValue;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
