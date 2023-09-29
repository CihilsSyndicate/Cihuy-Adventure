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
    public HealthBar healthBar;
    public GameObject healthHolder;
    public GameObject floatingTextDamage;

    private Animator anim;
    public float patrolDistance = 5f;
    public float detectionDistance = 3f; // Jarak untuk mendeteksi pemain
    public float speed = 2f;
    public float patrolDelay = 2f;
    private float patrolStartTime;
    private bool inPatrolDelay = false;
    private bool alive = true;

    private Rigidbody2D myRb;
    private Transform target;

    private SpriteRenderer spriteRenderer;
    public float damageEffectDuration = 0.2f;

    private Vector3 initialPatrolPosition;
    private Vector3 targetPatrolPosition;
    private float distanceToPlayer;

    private void Start()
    {
        healthBar.SetMaxHealth(maxHealth);
        target = GameObject.FindWithTag("Player").transform;
        myRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPatrolPosition = transform.position;
        ChooseNewPatrolPosition();
        health = maxHealth.initialValue;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (inPatrolDelay && distanceToPlayer > detectionDistance )
        {
            anim.SetBool("isMoving", false);
            if (Time.time - patrolStartTime >= patrolDelay)
            {
                inPatrolDelay = false;
                ChooseNewPatrolPosition();
            }
        }
        else if (alive && enemyState != EnemyState.Stagger)
        {
           
            if (distanceToPlayer <= detectionDistance)
            {
                // Pemain dalam jangkauan, dekati pemain
                anim.SetBool("isMoving", true);
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {
                // Pemain di luar jangkauan, lanjutkan patroli
                anim.SetBool("isMoving", true);
                transform.position = Vector3.MoveTowards(transform.position, targetPatrolPosition, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPatrolPosition) < 0.1f)
                {
                    inPatrolDelay = true;
                    patrolStartTime = Time.time;
                }
            }
        }
    }

    private void ChooseNewPatrolPosition()
    {
        float randomX = Random.Range(-patrolDistance, patrolDistance);
        float randomY = Random.Range(-patrolDistance, patrolDistance);

        // Menggunakan Vector3 dengan nilai Z tetap 0
        targetPatrolPosition = initialPatrolPosition + new Vector3(randomX, randomY, 0f);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        ShowFloatingText(damage);
        StartCoroutine(DamageEffect());
        anim.SetTrigger("hit");
        if (health <= 0)
        {
            alive = false;
            anim.SetBool("isAlive", false);
            Destroy(gameObject);
        }
        healthHolder.SetActive(true);
    }

    public void ShowFloatingText(float damage)
    {
        var go = Instantiate(floatingTextDamage, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "-" + damage.ToString();
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

    private IEnumerator DamageEffect()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(damageEffectDuration);

        spriteRenderer.color = Color.white;
    }
}
