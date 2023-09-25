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

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootAllEnemies();
            nextFireTime = Time.time + attackSpeed / fireRate;
        }
    }

    void ShootAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < maxShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = (enemies[i].transform.position - firePoint.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
    }
}
