using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour
{
    [Header("Movement and Physics")]
    private Rigidbody2D rb;
    private float moveSpeed;
    private bool isMoving;
    private float interval;
    private float intervalCounter;
    private float moveDuration;
    private float moveDurationCounter;
    private Vector3 moveDirection;
    public float damageEffectDuration = 0.2f;
    private SpriteRenderer spriteRenderer;
    [System.NonSerialized] public bool isKnockback;

    [Header("Attacking")]
    public GameObject slimeBulletPrefab;
    private GameObject bulletContainer;
    public GameObject floatingTextDamage;

    [Header("Health")]
    public FloatValue maxHealth;
    public float health;
    public HealthBar healthBar;
    public GameObject healthHolder;

    [Header("Item Drop")]
    public GameObject coinPrefab;


    private void Awake()
    {
       health = maxHealth.initialValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletContainer = GameObject.Find("BulletContainer");
        InvokeRepeating("Shoot", Random.Range(4f, 10f), Random.Range(9f, 11f));
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = Random.Range(1f, 2.5f);
        interval = Random.Range(1f, 3f);
        moveDuration = Random.Range(0.5f, 2f);

        intervalCounter = interval;
        moveDurationCounter = moveDuration;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
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
            if (!isKnockback)
            {
                rb.velocity = Vector2.zero;
            }

            if (intervalCounter < 0)
            {
                isMoving = true;
                moveDurationCounter = moveDuration;
                moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
            }
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
        healthBar.SetHealth(health);
        isKnockback = true;
        healthHolder.SetActive(true);
        StartCoroutine(DamageEffect());
        if (health <= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject coin = Instantiate(coinPrefab);
                coin.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
        if(floatingTextDamage)
        {
            ShowFloatingText(damage);
        }

    }

    public void ShowFloatingText(float damage)
    {
        var go = Instantiate(floatingTextDamage, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "-" + damage.ToString();
    }

    private IEnumerator DamageEffect()
    {
        spriteRenderer.color = Color.red; // Mengubah warna menjadi merah

        yield return new WaitForSeconds(damageEffectDuration);

        spriteRenderer.color = Color.white; // Mengembalikan warna aslinya
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
            isKnockback = false;
            //myRb.velocity = Vector2.zero;
        }
    }
}