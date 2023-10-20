using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void EnterGame()
    {
        GameManager.Instance.LoadGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Destroy(PlayerMovement.Instance.gameObject);
    }

    public void FollowUP()
    {
        Application.OpenURL("https://wa.me/6281973536858?text=Halo%20Farid%20Admin%20Cihuy%20Goes%20to%20school");
    }

    public void RateUS()
    {
        Application.OpenURL("https://wa.me/6282116375827?text=Waw%20Game%20Cihuy%20Ini%20Sangat%20Bagus%20Sekali");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
