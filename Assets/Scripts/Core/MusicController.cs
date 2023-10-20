using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [SerializeField] private string[] scenesToMute;

    private AudioSource musicAudioSource;

    private bool isMuted = false;

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        foreach (string sceneToMute in scenesToMute)
        {
            if (sceneName == sceneToMute)
            {
                MuteMusic(true);
                return;
            }
        }

        MuteMusic(false);
    }

    private void MuteMusic(bool mute)
    {
        isMuted = mute;
        musicAudioSource.mute = isMuted;
    }

    public void ToggleMusic()
    {
        MuteMusic(!isMuted);
    }
}
