using UnityEngine;
using System.Collections.Generic;

public class SlimeSpawner : MonoBehaviour
{
    [Header("Pooling")]
    public GameObject slimePrefab;
    public int poolSize = 5; // Jumlah awal objek dalam pool
    private List<GameObject> slimePool;
    private int currentPoolIndex = 0;
    public FloatValue slimeHp;

    [Header("Spawning")]
    public float spawnInterval; // Jeda antara setiap spawn (2 detik)
    public int maxSlimeCount;
    public bool isPaused = false;
    public bool isSpawning = true; // Untuk mengontrol apakah spawner sedang aktif atau tidak
    public float minSpawnDistance = 0f;
    public float maxSpawnDistance = 10f;
    [System.NonSerialized] public int currentSlimeCount = 0;
    private float timeSinceLastSpawn = 0f;

    private void Awake()
    {
        GameObject bulletContainer = GameObject.Find("BulletContainer");
        slimePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject slime = Instantiate(slimePrefab);
            slime.transform.SetParent(bulletContainer.transform);
            slime.SetActive(false);
            slimePool.Add(slime);
        }
    }

    private void Update()
    {
        SlimeController[] slimes = FindObjectsOfType<SlimeController>();
        currentSlimeCount = slimes.Length;

        if (isSpawning && !isPaused && currentSlimeCount < maxSlimeCount)
        {
            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnSlime();
                timeSinceLastSpawn = 0f;
                spawnInterval = Random.Range(2f, 4f);
            }
            timeSinceLastSpawn += Time.deltaTime;
        }

        if (currentSlimeCount >= maxSlimeCount)
        {
            isSpawning = false;
        }

        // Periksa apakah ada slime yang mati.
        if (!isSpawning && currentSlimeCount < maxSlimeCount)
        {
            isSpawning = true;
        }
    }

    private void SpawnSlime()
    {
        if (currentPoolIndex >= poolSize)
        {
            currentPoolIndex = 0; // Kembali ke awal pool jika sudah mencapai akhir
        }

        // Ambil objek dari pool
        GameObject newSlime = slimePool[currentPoolIndex];
        currentPoolIndex++;

        float spawnAngle = Random.Range(0f, 360f);
        float spawnRadius = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Hitung posisi spawn menggunakan koordinat polar
        float spawnX = transform.position.x + Mathf.Cos(spawnAngle * Mathf.Deg2Rad) * spawnRadius;
        float spawnY = transform.position.y + Mathf.Sin(spawnAngle * Mathf.Deg2Rad) * spawnRadius;
        float spawnZ = 0;

        // Setel posisi objek baru dan aktifkan
        newSlime.transform.position = new Vector3(spawnX, spawnY, spawnZ);
        newSlime.SetActive(true);
    }


    // Metode untuk memulai spawn
    public void StartSpawning()
    {
        isSpawning = true;
    }

    // Metode untuk berhenti spawn
    public void StopSpawning()
    {
        isSpawning = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minSpawnDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSpawnDistance);
    }
}
