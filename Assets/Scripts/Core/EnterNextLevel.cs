using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterNextLevel : MonoBehaviour
{
    public Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        ArrowSign.Instance.gameObject.SetActive(false);
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            ArrowSign.Instance.gameObject.SetActive(true);
            col.enabled = true;
        }
    }
}
