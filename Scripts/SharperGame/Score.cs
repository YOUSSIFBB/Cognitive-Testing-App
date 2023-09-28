using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public MouseCursor mouseCursor;

    public int score = 0;
    public string scoreKey = "score";

    private void Start()
    {
        // If the stored score is not 0, load it
        if (PlayerPrefs.HasKey(scoreKey))
        {
            score = PlayerPrefs.GetInt(scoreKey);
        }

        scoreText = GetComponent<Text>();
        UpdateScoreText();

        // Get reference to MouseCursor script
        mouseCursor = FindObjectOfType<MouseCursor>();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void UpdateScore()
    {
        if (MouseCursor.instance.isOverBlackSheep)
        {
            score++;
            UpdateScoreText();
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return score;
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.Save();
    }

    private void OnEnable()
    {
        //dont load score if its a new game
        if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            score = 0;
        }
        // Load the saved score from PlayerPrefs and update the score text
        else if (PlayerPrefs.HasKey(scoreKey))
        {
            score = PlayerPrefs.GetInt(scoreKey);
        }
        UpdateScoreText();
    }

    private void OnApplicationQuit()
    {
        // Reset the score when the game is closed
        ResetScore();
    }
}