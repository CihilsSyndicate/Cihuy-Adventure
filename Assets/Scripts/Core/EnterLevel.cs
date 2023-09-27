using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevel : MonoBehaviour
{
    public string nextLevel;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("UnlockedLevel") == 0)
        {
            PlayerPrefs.SetInt("UnlockedLevel", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
            string levelName;
            if (SceneManager.GetActiveScene().name == "Rest Area")
            {
                levelName = "Level " + unlockedLevel.ToString();
            }
            else
            {
                int nextLevel = unlockedLevel + 1;
                levelName = "Level " + nextLevel.ToString();
                if(SceneManager.GetActiveScene().buildIndex == PlayerPrefs.GetInt("UnlockedLevel") - 1)
                {
                    PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
                }
            }
            SceneManager.LoadScene(levelName);
        }
    }
}
