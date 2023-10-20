﻿using System.Collections;
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
        PlayerMovement.Instance.currentHealth.RuntimeValue = PlayerMovement.Instance.currentHealth.initialValue;
        SceneManager.LoadScene("Main Menu");
        PlayerPrefs.DeleteAll();
        playerInventory.myInventory.Clear();
        NpcSign.Instance.easterEggMakcik = 0;
        Destroy(gameObject);
    }

    public void RestartGame()
    {
        PlayerMovement.Instance.currentHealth.RuntimeValue = PlayerMovement.Instance.currentHealth.initialValue;
        Instantiate(gameObject);
        SceneManager.LoadScene("Home");
        PlayerPrefs.DeleteAll();
        playerInventory.myInventory.Clear();
        NpcSign.Instance.easterEggMakcik = 0;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
