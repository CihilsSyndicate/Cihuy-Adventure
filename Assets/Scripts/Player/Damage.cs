﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Knockback")]
    public float force;
    public float damage;
    public float knockTime;



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
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<SlimeController>().Knock(hit, knockTime, damage);
                }
                if (other.CompareTag("Enemy") && other.isTrigger)
                {                 
                    hit.GetComponent<HappySlime>().enemyState = EnemyState.Stagger;
                    other.GetComponent<HappySlime>().Knock(hit, knockTime, damage);
                }
                if (other.CompareTag("Player"))
                {
                    if(other.GetComponent<PlayerMovement>().currentState != playerState.stagger)
                    {
                        hit.GetComponent<PlayerMovement>().currentState = playerState.stagger;
                        other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                    
                }
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * force;
                hit.AddForce(difference, ForceMode2D.Impulse);
            }
            
        }
    
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            Vector2 difference = hit.transform.position - transform.position;
            difference = difference.normalized * force;
            hit.AddForce(difference, ForceMode2D.Impulse);
            if (other.GetComponent<PlayerMovement>().currentState != playerState.stagger)
            {
                hit.GetComponent<PlayerMovement>().currentState = playerState.stagger;
                other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
            }

        }
        
    }
}
