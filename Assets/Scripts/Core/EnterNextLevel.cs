using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterNextLevel : MonoBehaviour
{
    public Collider2D col;
    private GameObject arrowSign;
    // Start is called before the first frame update
    void Start()
    {
        arrowSign = GameObject.Find("ArrowSign");
        arrowSign.SetActive(false);
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            arrowSign.SetActive(true);
            col.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            arrowSign.SetActive(false);
    }
}
