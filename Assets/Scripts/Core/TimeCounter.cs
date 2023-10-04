using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public Text timeText;
    public float elapsedTime = 0f;
    private bool isCounting = false;

    private void Start()
    {
        isCounting = true;
        timeText = GetComponent<Text>();
    }

    private void Update()
    {
        if (isCounting)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeText();
        }
    }

    public void StartCounting()
    {
        isCounting = true;
    }

    public void StopCounting()
    {
        isCounting = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = timeString;
    }
}
