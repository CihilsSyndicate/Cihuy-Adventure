using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Walk,
    Idle,
    Stagger
}

public class HappySlime : MonoBehaviour
{

    public EnemyState enemyState;
    public float health;
    public FloatValue maxHealth;

    private Animator anim;
    public float patrolDistance = 5f; // Maximum distance the enemy will patrol
    public float speed = 2f; // Enemy movement speed
    public float patrolDelay = 2f; // Delay time before patrolling to the next position
    private float patrolStartTime;
    private bool inPatrolDelay = false;
    private bool alive = true;  

    private Vector2 initialPatrolPosition;
    private Vector2 targetPatrolPosition;

    private void Start()
    {
        initialPatrolPosition = transform.position;
        ChooseNewPatrolPosition();
        health = maxHealth.initialValue;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inPatrolDelay)
        {
            anim.SetBool("isMoving", false);
            // Check if the patrol delay time has elapsed
            if (Time.time - patrolStartTime >= patrolDelay)
            {
                inPatrolDelay = false;
                ChooseNewPatrolPosition();
            }
        }
        else if(alive && enemyState != EnemyState.Stagger)
        {
            anim.SetBool("isMoving", true);
            // Move towards the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPatrolPosition, speed * Time.deltaTime);

            // Check if we've reached the target position
            if (Vector2.Distance(transform.position, targetPatrolPosition) < 0.1f)
            {
                inPatrolDelay = true;
                patrolStartTime = Time.time;
            }
        }

    }

    private void ChooseNewPatrolPosition()
    {
        // Choose a new patrol position randomly within the specified distance
        float randomX = Random.Range(-patrolDistance, patrolDistance);
        float randomY = Random.Range(-patrolDistance, patrolDistance);
        targetPatrolPosition = initialPatrolPosition + new Vector2(randomX, randomY);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        anim.SetTrigger("hit");
        if(health <= 0)
        {
            alive = false;
            anim.SetBool("isAlive", false);
        }
    }

    public void Knock(Rigidbody2D myRb, float knockTime, float damage)
    {
        StartCoroutine(knockCo(myRb, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator knockCo(Rigidbody2D myRb, float knockTime)
    {
        if (myRb != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRb.velocity = Vector2.zero;
            enemyState = EnemyState.Idle;
            myRb.velocity = Vector2.zero;
        }
    }
}
