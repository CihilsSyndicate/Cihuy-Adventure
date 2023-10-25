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

    }

    public void SavePlayer()
    {
        if(PlayerMovement.Instance != null)
        {
            SaveSystem.SavePlayer(PlayerMovement.Instance);
        }
          
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
        if(data.health > 0)
            PlayerMovement.Instance.currentHealth.RuntimeValue = data.health;
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
