using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public float startTime = 45f;

    public float currentTime;
    public bool isTimeUp = false;

    public Text timerText;

    public void Start()
    {
        currentTime = startTime;
        timerText = GetComponent<Text>();
        UpdateTimerText();
    }

    public void Update()
    {
        if (isTimeUp)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isTimeUp = true;
        }
        UpdateTimerText();
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        string minutesString = minutes.ToString("00");
        string secondsString = seconds.ToString("00");

        timerText.text = minutesString + ":" + secondsString;
    }

    public bool IsTimeUp()
    {
        return isTimeUp;
    }
}