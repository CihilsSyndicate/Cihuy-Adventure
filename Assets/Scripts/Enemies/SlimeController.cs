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
        bulletContainer = GameObject.Find("BulletContainer");
        InvokeRepeating("Shoot", Random.Range(4f, 10f), Random.Range(9f, 11f));
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = Random.Range(1f, 2.5f);
        interval = Random.Range(1f, 3f);
        moveDuration = Random.Range(0.5f, 2f);

        intervalCounter = interval;
        moveDurationCounter = moveDuration;
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
            rb.velocity = Vector2.zero;

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
        if(gameObject.tag == "Enemy")
        {
            GameObject player = GameObject.Find("Player");

            if (player != null)
            {
                GameObject bullet = Instantiate(slimeBulletPrefab);
                bullet.transform.SetParent(bulletContainer.transform);
                bullet.transform.position = transform.position;
                Vector2 direction = player.transform.position - bullet.transform.position;
                bullet.GetComponent<BulletController>().SetDirection(direction);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {       
            Destroy(gameObject);
        }
    }
   
}