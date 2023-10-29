using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevel : MonoBehaviour
{
    public string nextScene;

    // Start is called before the first frame update
    void Init()
    {
        gameObject.tag = "Teleporter";
    }

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextScene);
            PlayerPrefs.SetString("LastScene", nextScene);
            PlayerPrefs.SetString("SpawnPoint", SceneManager.GetActiveScene().name);
            Init();

            if(SceneManager.GetActiveScene().name == "Boss Level")
            {
                PlayerMovement.Instance.bossHealthBar.gameObject.SetActive(false);
            }
        }
    }
}
