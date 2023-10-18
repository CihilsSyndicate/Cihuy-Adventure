using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour
{
    public int killingScore;

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
    public SpriteRenderer spriteRenderer;
    [System.NonSerialized] public bool isKnockback;
    private Transform player;

    [Header("Attacking")]
    public GameObject[] bulletPool;
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
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletContainer = GameObject.Find("BulletContainer");
        if(SceneManager.GetActiveScene().name != "SurvivalMode")
        {
            InvokeRepeating("Shoot", Random.Range(4f, 10f), Random.Range(9f, 11f));
        }
        else
        {
            InvokeRepeating("Shoot", Random.Range(1f, 2.5f), Random.Range(4f, 7f));
        }
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = Random.Range(1f, 2.5f);
        interval = Random.Range(1f, 3f);
        moveDuration = Random.Range(0.5f, 2f);

        intervalCounter = interval;
        moveDurationCounter = moveDuration;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnEnable()
    {
        healthBar.SetHealth(health);
        spriteRenderer.color = Color.white;
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
                if(SceneManager.GetActiveScene().name != "SurvivalMode")
                {
                    moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
                }
                else
                {
                    if (player != null)
                    {
                        moveDirection = (player.position - transform.position).normalized * moveSpeed;
                    }
                }
            }
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        return null; // Kembalikan null jika tidak ada peluru yang tersedia.
    }

    void Shoot()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            GameObject bullet = GetBullet(); // Menggunakan pool peluru
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                Vector2 direction = target.transform.position - bullet.transform.position;
                bullet.GetComponent<BulletController>().SetDirection(direction);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (floatingTextDamage)
        {
            ShowFloatingText(damage);
        }
        health -= damage;
        healthBar.SetHealth(health);
        isKnockback = true;
        healthHolder.SetActive(true);
        if (health <= 0)
        {
            if(SceneManager.GetActiveScene().name == "SurvivalMode")
            {
                ScoreManager.Instance.AddScore(killingScore);
            }
            for (int i = 0; i < 3; i++)
            {
                GameObject coin = Instantiate(coinPrefab);
                coin.transform.position = transform.position;
            }
            gameObject.SetActive(false);
            health = maxHealth.maxHealth;
        }       
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DamageEffect());
        }
    }

    public void ShowFloatingText(float damage)
    {
        var go = Instantiate(floatingTextDamage, transform.position, Quaternion.identity);
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