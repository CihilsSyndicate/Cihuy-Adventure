using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] Image musicOn;
    [SerializeField] Image musicOff;
    [SerializeField] private AudioSource musicAudioSource; // Gunakan AudioSource terpisah untuk musik
    [SerializeField] string[] scenesToMuteMusic;

    private bool muted = false;
    private static MusicManager musicManager;

    private void Awake()
    {
        if (musicManager == null)
        {
            musicManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Dapatkan nama scene yang sedang aktif saat ini
        string activeScene = SceneManager.GetActiveScene().name;

        // Periksa apakah nama scene ada dalam array yang akan mematikan musik
        foreach (string sceneName in scenesToMuteMusic)
        {
            if (activeScene == sceneName)
            {
                musicAudioSource.mute = true;
                return;
            }
        }
    }

    private void Start()
    {
        // Dapatkan nama scene yang sedang aktif saat ini
        string activeScene = SceneManager.GetActiveScene().name;

        // Periksa apakah nama scene ada dalam array yang akan mematikan musik
        foreach (string sceneName in scenesToMuteMusic)
        {
            if (activeScene == sceneName)
            {
                return; // Jika musik dimatikan pada scene saat ini, keluar dari metode Start
            }
        }

        musicAudioSource = GameObject.Find("Bgm").GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("musicMuted"))
        {
            PlayerPrefs.SetInt("musicMuted", 0);
            Load();
        }
        else
        {
            Load();
        }

        UpdateButtonIcon();
        musicAudioSource.mute = muted; // Gunakan mute pada AudioSource untuk mengontrol musik
    }


    public void OnButtonPress()
    {
        muted = !muted; // Toggle status muted
        musicAudioSource.mute = muted; // Mengubah mute pada AudioSource

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        musicOn.enabled = !muted;
        musicOff.enabled = muted;
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("musicMuted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("musicMuted", muted ? 1 : 0);
    }
}
