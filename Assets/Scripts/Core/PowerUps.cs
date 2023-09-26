using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    public GameObject popupSelectPowerUp;
    private bool enemiesDefeated = false;
    public GameObject[] powerUpGO;
    public GameObject orbitingSword;
    public int swordDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Periksa apakah tidak ada musuh yang tersisa dengan tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Periksa apakah tidak ada bos yang tersisa dengan tag "Boss"
        GameObject[] bossEnemies = GameObject.FindGameObjectsWithTag("Boss");

        // Gabungkan kedua array musuh dan bos menjadi satu array
        GameObject[] allEnemies = new GameObject[enemies.Length + bossEnemies.Length];
        enemies.CopyTo(allEnemies, 0);
        bossEnemies.CopyTo(allEnemies, enemies.Length);

        // Sekarang Anda memiliki semua musuh (termasuk bos) dalam array allEnemies
        if (allEnemies.Length == 0 && enemiesDefeated == false)
        {
            enemiesDefeated = true;
            ShowPowerUpPopUp();
        }

    }

    public void ShowPowerUpPopUp()
    {
        foreach (var powerUp in powerUpGO)
        {
            powerUp.SetActive(false);
        }
        // Pilih dua PowerUp secara acak dan aktifkan
        List<int> chosenIndices = new List<int>();
        int numberOfPowerUpsToShow = 2; // Ganti sesuai dengan jumlah yang Anda inginkan
        for (int i = 0; i < numberOfPowerUpsToShow; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, powerUpGO.Length);
            } while (chosenIndices.Contains(randomIndex));

            chosenIndices.Add(randomIndex);
            powerUpGO[randomIndex].SetActive(true);
        }
        popupSelectPowerUp.SetActive(true);
    }

    public void SelectPowerUp()
    {
        popupSelectPowerUp.SetActive(false);
    }

    public void IncreaseAspd()
    {
        PlayerAttack.Instance.attackSpeed -= 0.1f;
    }

    public void IncreaseMoveSpeed()
    {
        PlayerMovement.Instance.speed += 2f;
    }

    public void IncreaseMaxShot()
    {
        PlayerAttack.Instance.maxShot += 1;
    }

    public void AddSword()
    {
        StartCoroutine(ActivateSwordForDuration());
    }

    private IEnumerator ActivateSwordForDuration()
    {
        orbitingSword.SetActive(true);
        yield return new WaitForSeconds(swordDuration);
        orbitingSword.SetActive(false);
    }
}
