using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePoint; // Posisi dari mana peluru ditembakkan.
    public GameObject bulletPrefab; // Prefab peluru.
    public float bulletSpeed = 10f; // Kecepatan peluru.
    public float fireRate = 3f; // Kecepatan tembakan per detik.
    public float attackSpeed;
    private float nextFireTime = 0f; // Waktu berikutnya pemain bisa menembak.
    public int maxShot;
    private GameObject bulletContainer;
    public float shootRange;
    [System.NonSerialized] public Animator playerAnim;
    public int poolSize = 10; // Jumlah peluru dalam pool.
    private List<GameObject> bulletPool = new List<GameObject>();

    private static PlayerAttack instance;

    public static PlayerAttack Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        bulletContainer = GameObject.Find("BulletContainer");
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            if(bulletContainer != null)
                bullet.transform.SetParent(bulletContainer.transform);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
        playerAnim = GetComponent<Animator>();
        // Inisialisasi instance singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Hancurkan objek jika instance sudah ada
        }
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, shootRange, LayerMask.GetMask("Enemy", "Boss"));

        if (colliders.Length > 0 && Time.time >= nextFireTime)
        {
            ShootAllEnemies(colliders);
            nextFireTime = Time.time + attackSpeed / fireRate;
        }
    }

    void ShootAllEnemies(Collider2D[] colliders)
    {
        for (int i = 0; i < Mathf.Min(maxShot, colliders.Length); i++)
        {
            GameObject bullet = GetPooledBullet();
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                bullet.SetActive(true);
                
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 direction = (colliders[i].transform.position - firePoint.position).normalized;
                rb.velocity = direction * bulletSpeed;
            }
        }
    }

    GameObject GetPooledBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null; // Return null if no available bullets in the pool.
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
