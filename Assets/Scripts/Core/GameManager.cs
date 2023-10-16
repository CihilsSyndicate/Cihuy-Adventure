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
        Debug.Log(PlayerPrefs.GetString("LastScene"));
        if(PlayerPrefs.GetString("EquippedWeaponName") != null)
        {
            WeaponManager.Instance.LoadEquippedWeapon();
        }
    }

    public void SavePlayer()
    {
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
        Instantiate(playerPrefab);
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
