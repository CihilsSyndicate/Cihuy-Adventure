using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    float knockbackDuration;
    float knockbackPower;
    public Transform player;
    private Rigidbody2D rb;
    public GameObject enemy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator KnockbackCo()
    {
        float timer = 0;
        while(knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (player.transform.position - enemy.transform.position).normalized;
            rb.AddForce(-direction * knockbackPower);
        }
        yield return 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "BouncingSlime")
        {
            StartCoroutine(KnockbackCo());
        }
    }
}
