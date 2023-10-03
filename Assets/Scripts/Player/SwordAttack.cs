using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwordAttack : MonoBehaviour
{
    private Animator swordAnim;
    public GameObject slashPrefab; // Prefab Slash
    public float attackRange = 6f;
    public float speed = 5f;
    public float attackCooldown = 0.2f;
    private bool isAttacking = false;
    public int maxShot = 1;
    private float lastAttackTime = 0f;
    [System.NonSerialized] public bool isSurvivalMode;
    private GameObject bulletContainer;

    private static SwordAttack instance;

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
        swordAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SurvivalMode")
        {
            isSurvivalMode = true;
        }
        else
        {
            isSurvivalMode = false;
        }
        
        swordAnim.SetFloat("idleX", PlayerMovement.Instance.change.x);
        swordAnim.SetFloat("idleY", PlayerMovement.Instance.change.y);

        // Cek apakah pemain dapat menyerang
        if (!isAttacking)
        {
            // Cek waktu sekarang
            float currentTime = Time.time;

            // Cek apakah sudah cukup waktu untuk melakukan serangan lagi
            if (currentTime - lastAttackTime >= attackCooldown)
            {
                // Cek apakah pemain sedang bergerak atau isSurvivalMode true
                if (PlayerMovement.Instance.currentState != playerState.walk || isSurvivalMode)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy", "Boss"));

                    if (colliders.Length > 0)
                    {
                        Transform nearestEnemy = FindNearestEnemy(colliders);

                        if (nearestEnemy != null)
                        {
                            Vector3 direction = nearestEnemy.position - transform.position;

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

                            if (!isSurvivalMode)
                            {
                                PlayerAttack.Instance.playerAnim.SetFloat("x", posX);
                                PlayerAttack.Instance.playerAnim.SetFloat("y", posY);
                            }

                            swordAnim.SetFloat("x", posX);
                            swordAnim.SetFloat("y", posY);

                            // Menandai waktu serangan terakhir
                            lastAttackTime = currentTime;

                            // Menyerang
                            PerformAttack(colliders);
                        }
                    }
                }
            }
        }
    }


    private void PerformAttack(Collider2D[] colliders)
    {
        isAttacking = true;
        swordAnim.SetBool("IsAttacking", true);

        for (int i = 0; i < Mathf.Min(maxShot, colliders.Length); i++)
        {
            GameObject slashInstance = Instantiate(slashPrefab, transform.position, Quaternion.identity);
            slashInstance.transform.SetParent(bulletContainer.transform);
            Rigidbody2D rb = slashInstance.GetComponent<Rigidbody2D>();
            Vector2 direction = (colliders[i].transform.position - transform.position).normalized;
            rb.velocity = direction * speed;

            Slash slashScript = slashInstance.GetComponent<Slash>();
            slashScript.SetDirection(direction);
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
}
