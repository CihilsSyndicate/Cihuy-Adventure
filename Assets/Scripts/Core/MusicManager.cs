using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    [SerializeField] Image MusicOn;
    [SerializeField] Image MusicOff;
    [SerializeField] private AudioSource BacksoundMusic;
    private bool musicMuted = false;
    private static MusicManager musicManager;

    void awake()
    {
        if (musicManager == null)
        {
            musicManager = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(!PlayerPrefs.HasKey("musicMuted"))
        {
            PlayerPrefs.SetInt("musicMuted", 0); 
            Load();
        }

        else
        {
            Load();
        }

        UpdateButtonIcon();
        AudioListener.pause = musicMuted;
    }

    public void OnButtonPress()
    {
        if (musicMuted == false)
        {
            musicMuted = true;
            AudioListener.pause = true;
        }

        else
        {
            musicMuted = false;
            AudioListener.pause = false;
        }

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if(musicMuted == false)
        {
            MusicOn.enabled = true;
            MusicOff.enabled = false;
        }

        else
        { 
            MusicOn.enabled = false;
            MusicOff.enabled = true;
        }
    }

    // Change this line in MusicManager
    private void Load()
    {
        musicMuted = PlayerPrefs.GetInt("musicMuted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("musicMuted", musicMuted ? 1 : 0);
    }
}
