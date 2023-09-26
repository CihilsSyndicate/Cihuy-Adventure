using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappySlime : MonoBehaviour
{
    public float health;
    public FloatValue maxHealth;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth.initialValue;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        anim.SetTrigger("hit");
        if(health <= 0)
        {
            anim.SetBool("isAlive", false);
        }
    }
}
