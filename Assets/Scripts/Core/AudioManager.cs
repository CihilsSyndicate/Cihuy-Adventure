using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    [SerializeField] Image SoundOn; 
    [SerializeField] Image SoundOff;
    [SerializeField] private AudioSource SoundButton;
    private bool soundMuted = false;
    private static AudioManager audioManager;

    void awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(!PlayerPrefs.HasKey("soundMuted"))
        {
            PlayerPrefs.SetInt("soundMuted", 0); 
            Load();
        }

        else
        {
            Load();
        }

        UpdateButtonIcon();
        AudioListener.pause = soundMuted;
    }

    public void OnButtonPress()
    {
        if (soundMuted == false)
        {
            soundMuted = true;
            AudioListener.pause = true;
        }

        else
        {
            soundMuted = false;
            AudioListener.pause = false;
        }

        Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if(soundMuted == false)
        {
            SoundOn.enabled = true;
            SoundOff.enabled = false;
        }

        else
        {
            SoundOn.enabled = false;
            SoundOff.enabled = true;
        }
    }

    // Change this line in AudioManager
    private void Load()
    {
        soundMuted = PlayerPrefs.GetInt("soundMuted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("soundMuted", soundMuted ? 1 : 0);
    }

}
