using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwordAttack : MonoBehaviour
{
    [Header("General/Not Survival Mode")]
    public float attackButtonCooldown;
    private Animator swordAnim;
    public GameObject slashPrefab;
    public Button attackButton;
    private GameObject bulletContainer;

    [Header("Survival Mode")]
    public float attackRange = 6f;
    public float speed = 5f;
    public float attackCooldown = 0.2f;
    private bool isAttacking = false;
    public int maxShot = 1;
    private float lastAttackTime = 0f;

    [Header("Pooling System")]
    public int poolSize = 10;
    private List<GameObject> slashPool;

    private static SwordAttack instance;
    public AudioSource swordSlashAudio;


    public static SwordAttack Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bulletContainer = GameObject.Find("BulletContainer");
        InitializePool();
        swordAnim = GetComponent<Animator>();
    }

    public void InitializePool()
    {
        slashPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject slash = Instantiate(slashPrefab);
            if(bulletContainer != null)
                slash.transform.SetParent(bulletContainer.transform);
            slash.SetActive(false);
            slashPool.Add(slash);
        }
    }

    public GameObject GetSlash()
    {
        foreach (GameObject slash in slashPool)
        {
            if (!slash.activeInHierarchy)
            {
                slash.SetActive(true);
                return slash;
            }
        }

        return null; // Kembali null jika tidak ada peluru yang tersedia.
    }

    void Update()
    {
        swordAnim.SetFloat("idleX", PlayerMovement.Instance.change.x);
        swordAnim.SetFloat("idleY", PlayerMovement.Instance.change.y);

        if (!isAttacking)
        {
            // Cek waktu sekarang
            float currentTime = Time.time;

            // Cek apakah sudah cukup waktu untuk melakukan serangan lagi
            if (currentTime - lastAttackTime >= attackCooldown)
            {
                // Cek apakah pemain sedang bergerak atau isSurvivalMode true
                if (PlayerMovement.Instance.currentState != playerState.walk && SceneManager.GetActiveScene().name == "SurvivalMode")
                {
                    lastAttackTime = currentTime;
                    LaunchAttack();
                }
            }
        }
    }

    public void LaunchAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy", "Boss"));

        if (colliders.Length > 0)
        {
            Transform nearestEnemy = FindNearestEnemy(colliders);

            if (nearestEnemy != null)
            {
                swordSlashAudio.Play();
                Vector3 direction = (nearestEnemy.position - transform.position).normalized;

                float posX = 0f;
                float posY = 0f;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    posX = Mathf.Sign(direction.x);
                }
                else
                {
                    posY = Mathf.Sign(direction.y);
                }

                // Sinkronkan arah pemain, pedang, dan slash
                PlayerMovement.Instance.change.x = posX;
                PlayerMovement.Instance.change.y = posY;

                if (SceneManager.GetActiveScene().name != "SurvivalMode")
                {
                    PlayerAttack.Instance.playerAnim.SetFloat("x", posX);
                    PlayerAttack.Instance.playerAnim.SetFloat("y", posY);
                }

                swordAnim.SetFloat("x", posX);
                swordAnim.SetFloat("y", posY);

                // Menyerang
                StartCoroutine(PerformAttack(colliders, direction));
            }
        }
    }

    private IEnumerator PerformAttack(Collider2D[] colliders, Vector3 direction)
    {
        isAttacking = true;

        if (SceneManager.GetActiveScene().name != "SurvivalMode")
        {
            attackButton.interactable = false;
            swordAnim.SetBool("IsAttacking", true);
        }

        for (int i = 0; i < Mathf.Min(maxShot, colliders.Length); i++)
        {
            GameObject slashInstance = GetSlash();
            if (slashInstance != null)
            {
                slashInstance.transform.position = transform.position;

                Rigidbody2D rb = slashInstance.GetComponent<Rigidbody2D>();
                Vector2 slashDirection = new Vector2(direction.x, direction.y); 

                rb.velocity = slashDirection * speed;

                Slash slashScript = slashInstance.GetComponent<Slash>();
                slashScript.SetDirection(slashDirection);
            }
        }

        if (SceneManager.GetActiveScene().name == "SurvivalMode")
        {
            yield return new WaitForSeconds(0.2f);
            ResetIsAttacking();
        }
        else
        {
            yield return new WaitForSeconds(attackButtonCooldown);
            attackButton.interactable = true;
        }
    }


    // Metode untuk mengakhiri serangan
    private void ResetIsAttacking()
    {
        isAttacking = false;
        swordAnim.SetBool("IsAttacking", false);
    }

    public Transform FindNearestEnemy(Collider2D[] colliders)
    {
        Transform nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        Vector2 playerPosition = transform.position;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Boss"))
            {
                Vector2 enemyPosition = collider.transform.position;
                float distanceToEnemy = Vector2.Distance(playerPosition, enemyPosition);

                if (distanceToEnemy < nearestDistance)
                {
                    nearestDistance = distanceToEnemy;
                    nearestEnemy = collider.transform;
                }
            }
        }

        return nearestEnemy;
    }

    public void ClearSlashPool()
    {
        if(slashPool != null)
        {
            foreach (GameObject slash in slashPool)
            {
                Destroy(slash);
            }

            slashPool.Clear();
        }
    }
       

}
