using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InvokeRepeating("SavePlayer", 0f, 1f);
        if(PlayerPrefs.GetString("EquippedWeaponName") != null)
        {
            if(SceneManager.GetActiveScene().name != "Main Menu")
                WeaponManager.Instance.LoadEquippedWeapon();
        }
    }

    public void SavePlayer()
    {
        if(PlayerMovement.Instance != null)
            SaveSystem.SavePlayer(PlayerMovement.Instance);
    }

    public void LoadGame()
    {
        PlayerPrefs.DeleteKey("SpawnPoint");
        if(PlayerPrefs.GetString("LastScene") != "")
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("LastScene"));
        }
        else
        {
            SceneManager.LoadScene("Home");
            PlayerPrefs.SetString("LastScene", "Home");
        }
        if (PlayerMovement.Instance == null)
        {
            Instantiate(playerPrefab);
        }
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        PlayerMovement.Instance.currentHealth.RuntimeValue = data.health;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        PlayerMovement.Instance.transform.position = position;
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
   
}
