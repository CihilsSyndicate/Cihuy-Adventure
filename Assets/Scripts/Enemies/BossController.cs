﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("Movement and Physics")]
    public Rigidbody2D rb;
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
    public GameObject target;
    public GameObject slimeBulletPrefab;
    private GameObject bulletContainer;

    [Header("Health")]
    public FloatValue maxHealth;
    private HealthBar healthBar;


    
    private void Awake()
    {
        maxHealth.RuntimeValue = maxHealth.initialValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletContainer = GameObject.Find("BulletContainer");

        rb = GetComponent<Rigidbody2D>();

        interval = Random.Range(1f, 1.5f);
        moveDuration = Random.Range(0.5f, 1f);

        intervalCounter = interval;
        moveDurationCounter = moveDuration;
        bossAnim.SetBool("IsAlive", true);

        StartCoroutine(InitialDelay(3f));
        BossSlimeData bossSlimeData = SaveSystem.LoadBossSlime();
        if (bossSlimeData.health > 0)
        {
            maxHealth.RuntimeValue = bossSlimeData.health;
        }
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

                    if (target != null)
                    {
                        // Hitung vektor dari posisi bos ke posisi pemain
                        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
                    }
                    else
                    {
                        moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            moveDurationCounter = 0;
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(0f);
        }
    }

    void Shoot()
    {
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
        if(healthBar == null && PlayerMovement.Instance != null)
        {
            healthBar = PlayerMovement.Instance.bossHealthBar;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.gameObject.SetActive(true);
        }
        maxHealth.RuntimeValue -= damage;
        PlayerMovement.Instance.bossHealthBarText.text = maxHealth.RuntimeValue.ToString() + " / " + maxHealth.maxHealth.ToString(); ;
        healthBar.SetHealth(maxHealth.RuntimeValue);
        StartCoroutine(DamageEffect());
        SaveSystem.SaveBossSlime(this);
        if (maxHealth.RuntimeValue <= 0)
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