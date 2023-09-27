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
    public float shootRange = 5f;

    private static PlayerAttack instance;

    public static PlayerAttack Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        bulletContainer = GameObject.Find("BulletContainer");
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
        // Loop melalui semua musuh dan bos yang terdeteksi
        for (int i = 0; i < Mathf.Min(maxShot, colliders.Length); i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.SetParent(bulletContainer.transform);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = (colliders[i].transform.position - firePoint.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
    }
}
