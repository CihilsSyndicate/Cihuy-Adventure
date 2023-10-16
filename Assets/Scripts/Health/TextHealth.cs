using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHealth : MonoBehaviour
{
    public Text textCurrentHealth;
    public Text textMaxHealth;
    public FloatValue currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        textCurrentHealth.text = currentHealth.RuntimeValue.ToString();
        textMaxHealth.text = currentHealth.initialValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        textCurrentHealth.text = currentHealth.RuntimeValue.ToString();
    }
}
