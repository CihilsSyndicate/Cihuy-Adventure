using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BackToMainMenu()
    {
        PlayerMovement.Instance.currentHealth.RuntimeValue = PlayerMovement.Instance.currentHealth.maxHealth;
        SceneManager.LoadScene("Main Menu");
        Destroy(gameObject);
    }

    public void RestartGame()
    {
        PlayerMovement.Instance.currentHealth.RuntimeValue = PlayerMovement.Instance.currentHealth.maxHealth;
        GameManager.Instance.LoadGame();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
