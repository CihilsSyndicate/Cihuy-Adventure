using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHealth : MonoBehaviour
{
    public Text hpText;
    public FloatValue currentHealth;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = currentHealth.RuntimeValue.ToString() + " / " + currentHealth.initialValue.ToString();
    }
}
