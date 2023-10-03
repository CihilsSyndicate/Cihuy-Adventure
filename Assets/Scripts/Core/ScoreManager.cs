using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // UI Text untuk menampilkan skor
    [System.NonSerialized] public int score = 0; // Variabel untuk menyimpan skor pemain

    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Metode untuk menambah skor
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    // Metode untuk memperbarui teks skor di antarmuka pengguna
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
