using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    [Header("Magnetting")]
    private Transform playerTransform; // Referensi ke Transform pemain
    public float moveSpeed = 10f; // Kecepatan pergerakan koin
    public float delayBeforeMove = 1f; // Penundaan sebelum koin mulai bergerak
    private bool isMoving = false;
    private Collider2D coinCollider;
    private bool isSurvivalMode;
    public int coinValue;

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
        if (!isSurvivalMode)
        {
            coinTrailRenderer.enabled = false;
            coinCollider = GetComponent<Collider2D>();
            coinCollider.enabled = false;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "SurvivalMode")
        {
            isSurvivalMode = true;
        }
        else
        {
            isSurvivalMode = false;
        }

        if(when >= delay)
        {
            pastTime = Time.deltaTime;
            objectTransform.position += off * Time.deltaTime;
            delay += pastTime;
        }

        if(!isMoving)
        {
            StartCoroutine(StartMovingCoin());
        }
        else
        {
            if(playerTransform != null)
            {
                coinCollider.enabled = true;
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator StartMovingCoin()
    {
        yield return new WaitForSeconds(delayBeforeMove);
        coinTrailRenderer.enabled = true;
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            CoinCounter.Instance.IncreaseCoin(coinValue);
        }
    }
}
