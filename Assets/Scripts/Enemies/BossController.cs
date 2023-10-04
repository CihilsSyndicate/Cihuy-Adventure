using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("Movement and Physics")]
    private Rigidbody2D rb;
    public float moveSpeed;
    private bool isMoving;
    private float interval;
    private float intervalCounter;
    private float moveDuration;
    private float moveDurationCounter;
    private Vector3 moveDirection;
    public Animator bossAnim;
    public float damageEffectDuration = 0.2f;
    private SpriteRenderer spriteRenderer;
    private bool isWaiting;

    [Header("Attacking")]
    public GameObject slimeBulletPrefab;
    private GameObject bulletContainer;

    [Header("Health")]
    public FloatValue maxHealth;
    public float health;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletContainer = GameObject.Find("BulletContainer");
        // InvokeRepeating("Shoot", 5f, 1f);
        rb = GetComponent<Rigidbody2D>();

        interval = Random.Range(1f, 1.5f);
        moveDuration = Random.Range(0.5f, 1f);

        intervalCounter = interval;
        moveDurationCounter = moveDuration;
        bossAnim.SetBool("IsAlive", true);

        StartCoroutine(InitialDelay(3f));
    }

    IEnumerator InitialDelay(float delayTime)
    {
        isWaiting = true;

        yield return new WaitForSeconds(delayTime);

        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            if (isMoving)
            {
                moveDurationCounter -= Time.deltaTime;
                rb.velocity = moveDirection;

                if (moveDurationCounter < 0)
                {
                    isMoving = false;
                    intervalCounter = interval;
                }
            }
            else
            {
                intervalCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (intervalCounter < 0)
                {
                    isMoving = true;
                    moveDurationCounter = moveDuration;

                    // Cari GameObject dengan tag "Player"
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    if (player != null)
                    {
                        // Hitung vektor dari posisi bos ke posisi pemain
                        moveDirection = (player.transform.position - transform.position).normalized * moveSpeed;
                    }
                    else
                    {
                        moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            moveDurationCounter = 0;
        }
    }

    void Shoot()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            GameObject bullet = Instantiate(slimeBulletPrefab);
            bullet.transform.SetParent(bulletContainer.transform);
            bullet.transform.position = transform.position;
            Vector2 direction = target.transform.position - bullet.transform.position;
            bullet.GetComponent<BulletController>().SetDirection(direction);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(DamageEffect());
        if (health <= 0)
        {
            rb.velocity = Vector2.zero;
            moveDurationCounter = 0;
            bossAnim.SetBool("IsAlive", false);
        }
    }

    public void VanishBoss()
    {
        Destroy(gameObject);
    }

    private IEnumerator DamageEffect()
    {
        spriteRenderer.color = Color.red; // Mengubah warna menjadi merah

        yield return new WaitForSeconds(damageEffectDuration);

        spriteRenderer.color = Color.white; // Mengembalikan warna aslinya
    }
}