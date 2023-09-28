using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 8;
    public int maxLevel = 10;
    public bool pnlActive = false;

    private CountDownTimer countDownTimer;
    public static GameManager instance;
    public Score scoreScript;

    //make mouse cursor for game
    public GameObject MouseCursorPrefabulous;

    //score for the end screen panel
    public GameObject gameMenuPanel;

    public GameObject gamePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //find the end score of previous level
        scoreScript = FindObjectOfType<Score>();
    }

    private void Start()
    {
        scoreScript.ResetScore();
        Time.timeScale = 1;
        countDownTimer = FindObjectOfType<CountDownTimer>();

        // Instantiate the Mouse Cursor Prefab at the center of the screen
        GameObject mouseCursor = Instantiate(MouseCursorPrefabulous);
        GameObject canvas = GameObject.Find("Canvas");
        mouseCursor.transform.SetParent(canvas.transform, false);
        mouseCursor.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }

    private void Update()
    {
        if (countDownTimer.isTimeUp)
        {
            LoadNextLevel();
        }

        // Quit the game if the Escape key is pressed
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            gameMenuPanel.SetActive(true);
            gamePanel.SetActive(false);
            pnlActive = true;
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void LoadNextLevel()
    {
        if (currentLevel == 8)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(9);
        }
        else if (currentLevel == 9)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(10);
        }
        else
        {
            Time.timeScale = 0;
            gameMenuPanel.SetActive(true);
            gamePanel.SetActive(false);
            pnlActive = true;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 8)
        {
            scoreScript.ResetScore();
        }
        countDownTimer = FindObjectOfType<CountDownTimer>();
    }

    private void OnDisable()
    {
        if (scoreScript != null && currentLevel < SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt(scoreScript.scoreKey, scoreScript.GetScore());
            PlayerPrefs.Save();
        }
    }
}