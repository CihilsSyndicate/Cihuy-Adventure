using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource bgm;
    public AudioSource[] sfx;
    // public GameManager gameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicMuteStatus"))
        {
            if (PlayerPrefs.GetInt("MusicMuteStatus") == 1)
            {
                MuteBGM(true);
            }
            else if (PlayerPrefs.GetInt("MusicMuteStatus") == 0)
            {
                MuteBGM(false);
            }
        }

        if (PlayerPrefs.HasKey("SFXMuteStatus"))
        {
            int sfxMuteStatus = PlayerPrefs.GetInt("SFXMuteStatus");
            if (sfxMuteStatus == 0)
            {
                MuteSFX(false);
            }
            else
            {
                MuteSFX(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static SoundManager GetInstance()
    {
        return Instance;
    }

    public void MuteBGM(bool activate)
    {
        bgm.mute = activate;
    }

    public void MuteSFX(bool activate)
    {
        foreach (AudioSource audioSource in sfx)
        {
            audioSource.mute = activate;
        }
    }
}
