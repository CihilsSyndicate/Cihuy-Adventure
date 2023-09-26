using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Damage")]
    public float damage;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            other.collider.GetComponent<SlimeController>().TakeDamage(damage);
        }
        if(other.gameObject.name == "HappySlime")
        {
            other.collider.GetComponent<HappySlime>().TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "HappySlime")
        {
            other.GetComponent<HappySlime>().TakeDamage(damage);
        }
    }
}
