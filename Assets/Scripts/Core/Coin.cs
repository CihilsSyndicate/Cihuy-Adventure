using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Magnetting")]
    private Transform playerTransform; // Referensi ke Transform pemain
    public float moveSpeed = 10f; // Kecepatan pergerakan koin
    public float delayBeforeMove = 1f; // Penundaan sebelum koin mulai bergerak
    private bool isMoving = false;
    private Collider2D coinCollider;

    [Header("Random Splash")]
    public TrailRenderer coinTrailRenderer;
    public Transform objectTransform;
    private float delay = 0;
    private float pastTime = 0;
    private float when = 0.3f;
    private Vector3 off;

    private void Awake()
    {
        off = new Vector3(Random.Range(-3, 3), off.y, off.z);
        off = new Vector3(off.x, Random.Range(-3, 3), off.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        coinTrailRenderer = GetComponent<TrailRenderer>();
        coinTrailRenderer.enabled = false;
        coinCollider = GetComponent<Collider2D>();
        coinCollider.enabled = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(when >= delay)
        {
            pastTime = Time.deltaTime;
            objectTransform.position += off * Time.deltaTime;
            delay += pastTime;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bossEnemies = GameObject.FindGameObjectsWithTag("Boss");

        GameObject[] allEnemies = new GameObject[enemies.Length + bossEnemies.Length];
        enemies.CopyTo(allEnemies, 0);
        bossEnemies.CopyTo(allEnemies, enemies.Length);

        if (allEnemies.Length == 0 && isMoving == false)
        {
            coinTrailRenderer.enabled = true;
            isMoving = true;
        }

        if (isMoving)
        {
            coinCollider.enabled = true;
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            CoinCounter.Instance.IncreaseCoin(1);
        }
    }
}
