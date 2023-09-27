using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Magnetting")]
    private Transform playerTransform; // Referensi ke Transform pemain
    public float moveSpeed = 5f; // Kecepatan pergerakan koin
    public float delayBeforeMove = 2f; // Penundaan sebelum koin mulai bergerak
    private bool isMoving = false;

    [Header("Random Splash")]
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        StartCoroutine(StartMoving());
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

        if (isMoving)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator StartMoving()
    {
        // Tunggu selama delayBeforeMove
        yield return new WaitForSeconds(delayBeforeMove);

        // Setelah penundaan, aktifkan pergerakan koin
        isMoving = true;
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
