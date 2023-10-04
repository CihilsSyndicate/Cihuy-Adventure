using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // Prefab bos yang ingin Anda spawn
    private bool bossSpawned = false; // Menandakan apakah bos sudah di-spawn

    // Update is called once per frame
    void Update()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if(boss != null)
        {
            bossSpawned = true;
        }
        else
        {
            bossSpawned = false;
        }

        // Cek jika skor pemain mencapai 500 dan bos belum di-spawn
        if (ScoreManager.Instance.score == 300 && ScoreManager.Instance.score != 0 && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        // Buat instansi baru dari prefab bos di posisi yang diinginkan
        Instantiate(bossPrefab, transform.position, Quaternion.identity);

        // Setel bossSpawned ke true agar bos tidak di-spawn lagi
        bossSpawned = true;
    }
}
