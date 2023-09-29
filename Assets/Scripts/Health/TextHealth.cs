using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHealth : MonoBehaviour
{
    public TextMesh text;
    public FloatValue currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        text.text = currentHealth.RuntimeValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = currentHealth.RuntimeValue.ToString();
    }
}
