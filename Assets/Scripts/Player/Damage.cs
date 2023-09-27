using System.Collections;
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
        if(other.gameObject.name == "HappySlime" && other.CompareTag("Enemy"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                hit.GetComponent<HappySlime>().enemyState = EnemyState.Stagger;
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * force;
                hit.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(KnockCo(hit));
            }
            other.GetComponent<HappySlime>().TakeDamage(damage);
        }
        if (other.gameObject.name != "HappySlime" && other.CompareTag("Enemy"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * force;
                hit.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(KnockCo(hit));
            }
            other.GetComponent<SlimeController>().TakeDamage(damage);
        }
    }

    private IEnumerator KnockCo(Rigidbody2D hit)
    {
        if(hit != null)
        {
            yield return new WaitForSeconds(knockTime);
            hit.velocity = Vector2.zero;
            hit.GetComponent<HappySlime>().enemyState = EnemyState.Idle;
        }
    }
}
