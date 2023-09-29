using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Animator swordAnim;
    public GameObject slashPrefab; // Prefab Slash
    public float attackRange = 6f;
    public float speed = 5f;
    public float attackCooldown = 1f;
    private bool isAttacking = false;
    public int maxShot = 1;

    void Start()
    {
        swordAnim = GetComponent<Animator>();
    }

    void Update()
    {
        swordAnim.SetFloat("idleX", PlayerMovement.Instance.change.x);
        swordAnim.SetFloat("idleY", PlayerMovement.Instance.change.y);
        if (!isAttacking && PlayerMovement.Instance.currentState != playerState.walk)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy", "Boss"));

            if (colliders.Length > 0)
            {
                // Temukan musuh terdekat
                Transform nearestEnemy = PlayerAttack.Instance.FindNearestEnemy(colliders);

                if (nearestEnemy != null)
                {
                    // Menghitung perbedaan posisi antara pemain dan musuh
                    Vector3 direction = nearestEnemy.position - transform.position;

                    float posX = 0f;
                    float posY = 0f;

                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        posX = Mathf.Sign(direction.x);
                        Debug.Log(posX);
                    }
                    else
                    {
                        posY = Mathf.Sign(direction.y);
                        Debug.Log(posY);
                    }

                    // Mengatur parameter animasi "x" dan "y"
                    PlayerAttack.Instance.playerAnim.SetFloat("x", posX);
                    PlayerAttack.Instance.playerAnim.SetFloat("y", posY);
                    swordAnim.SetFloat("x", posX);
                    swordAnim.SetFloat("y", posY);

                    // Menyerang
                    Invoke("PerformAttack", attackCooldown);
                }
            }
        }
    }

    private void PerformAttack()
    {
        isAttacking = true;
        swordAnim.SetBool("IsAttacking", true);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy", "Boss"));

        for (int i = 0; i < Mathf.Min(maxShot, colliders.Length); i++)
        {
            GameObject slashInstance = Instantiate(slashPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = slashInstance.GetComponent<Rigidbody2D>();
            Vector2 direction = (colliders[i].transform.position - transform.position).normalized;
            rb.velocity = direction * speed;

            Slash slashScript = slashInstance.GetComponent<Slash>();
            slashScript.SetDirection(direction);
        }

        // Setelah delay attackCooldown, set isAttacking menjadi false dan matikan animasi serangan
        Invoke("FinishAttack", attackCooldown);
    }

    // Metode untuk mengakhiri serangan
    private void FinishAttack()
    {
        isAttacking = false;
        swordAnim.SetBool("IsAttacking", false);
    }
}
