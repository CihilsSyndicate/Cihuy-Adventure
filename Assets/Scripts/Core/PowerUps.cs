using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    public GameObject popupSelectPowerUp;
    private bool enemiesDefeated = false;
    public GameObject[] powerUpGO;
    public GameObject[] orbitingWeaponGO;
    private bool isSelectingPowerUp = false;
    private bool swordActivated = false;
    private bool orbitingWeaponActivated = false;
    public SlimeSpawner[] slimeSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name != "SurvivalMode")
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] bossEnemies = GameObject.FindGameObjectsWithTag("Boss");

            GameObject[] allEnemies = new GameObject[enemies.Length + bossEnemies.Length];
            enemies.CopyTo(allEnemies, 0);
            bossEnemies.CopyTo(allEnemies, enemies.Length);

            if (allEnemies.Length == 0 && enemiesDefeated == false)
            {
                enemiesDefeated = true;
                ShowPowerUpPopUp();
            }
        }
        else
        {
            if (ScoreManager.Instance.score % 30 == 0 && ScoreManager.Instance.score != 0 && !isSelectingPowerUp)
            {
                ScoreManager.Instance.AddScore(10);
                isSelectingPowerUp = true;
                ShowPowerUpPopUp();
                for (int i = 0; i < slimeSpawner.Length; i++)
                {
                    slimeSpawner[i].StopSpawning();
                }
            }
        }
    }

    public void ShowPowerUpPopUp()
    {
        Time.timeScale = 0;
        foreach (var powerUp in powerUpGO)
        {
            powerUp.SetActive(false);
        }

        // Pilih dua PowerUp secara acak dan aktifkan, kecuali jika orbitingWeapon sudah aktif
        List<int> chosenIndices = new List<int>();
        int numberOfPowerUpsToShow = 2;

        for (int i = 0; i < numberOfPowerUpsToShow; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, powerUpGO.Length);
            } while (chosenIndices.Contains(randomIndex) || (orbitingWeaponActivated && randomIndex == 5) || (swordActivated && randomIndex == 3));

            chosenIndices.Add(randomIndex);
            powerUpGO[randomIndex].SetActive(true);
        }
        isSelectingPowerUp = false;
        popupSelectPowerUp.SetActive(true);
    }


    public void SelectPowerUp()
    {
        popupSelectPowerUp.SetActive(false);     
        for (int i = 0; i < slimeSpawner.Length; i++)
        {
            slimeSpawner[i].StartSpawning();
        }
        Time.timeScale = 1;
    }

    public void IncreaseAspd(float amount)
    {
        PlayerAttack.Instance.attackSpeed -= amount;
    }

    public void IncreaseMoveSpeed(float amount)
    {
        PlayerMovement.Instance.speed += amount;
    }

    public void IncreaseFireBallMaxShot(int amount)
    {
        PlayerAttack.Instance.maxShot += amount;
    }

    public void IncreaseSwordSlashMaxShot(int amount)
    {
        SwordAttack.Instance.maxShot += amount;
    }

    public void AddSlashSword()
    {
        if (!swordActivated)
        {
            SwordAttack.Instance.enabled = true;
            swordActivated = true;
        }
    }

    public void IncreaseRangeAttack(float amount)
    {
        PlayerAttack.Instance.shootRange += amount;
    }

    public void AddOrbitingWeapon()
    {
        if (!orbitingWeaponActivated)
        {
            for (int i = 0; i < orbitingWeaponGO.Length; i++)
            {
                orbitingWeaponGO[i].SetActive(true);
            }

            orbitingWeaponActivated = true;
        }
    }

}
