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
    private bool isSelectingPowerUp;
    private bool swordActivated;
    private bool orbitingWeaponActivated = false;
    public SlimeSpawner[] slimeSpawner;
    List<int> chosenIndices;

    // Start is called before the first frame update
    void Start()
    {
        swordActivated = false;
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
            if (ScoreManager.Instance.score % 10 == 0 && ScoreManager.Instance.score != 0 && !isSelectingPowerUp)
            {
                for (int i = 0; i < slimeSpawner.Length; i++)
                {
                    slimeSpawner[i].StopSpawning();
                }
                isSelectingPowerUp = true;
                ShowPowerUpPopUp();
            }
        }

        Debug.Log(isSelectingPowerUp);
    }

    private bool ShouldExcludePowerUp(int randomIndex)
    {
        return chosenIndices.Contains(randomIndex) ||
               (PlayerMovement.Instance.speed > 12 && randomIndex == 0) ||
               (PlayerAttack.Instance.attackSpeed == 0.5f && randomIndex == 1) ||
               (PlayerAttack.Instance.maxShot == 4 && randomIndex == 2) ||
               (swordActivated && randomIndex == 3) ||
               (PlayerAttack.Instance.shootRange > 10f && randomIndex == 4) ||
               (orbitingWeaponActivated && randomIndex == 5) ||
               (SwordAttack.Instance.maxShot == 3 && randomIndex == 6) ||
               (SwordAttack.Instance.attackRange > 15f && randomIndex == 7);
    }

    public void ShowPowerUpPopUp()
    {       
        Time.timeScale = 0;
        foreach (var powerUp in powerUpGO)
        {
            powerUp.SetActive(false);
        }

        // Pilih dua PowerUp secara acak dan aktifkan, kecuali jika orbitingWeapon sudah aktif
        chosenIndices = new List<int>();
        int numberOfPowerUpsToShow = 2;

        for (int i = 0; i < numberOfPowerUpsToShow; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, powerUpGO.Length);
            } while (ShouldExcludePowerUp(randomIndex));

            chosenIndices.Add(randomIndex);
            powerUpGO[randomIndex].SetActive(true);
        }
        isSelectingPowerUp = true;
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
        isSelectingPowerUp = false;
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

    public void IncreaseSlashRangeShoot(float amount)
    {
        SwordAttack.Instance.attackRange += amount;
    }

    public void IncreaseSlashMaxShot(int amount)
    {
        SwordAttack.Instance.maxShot += amount;
    }
}
