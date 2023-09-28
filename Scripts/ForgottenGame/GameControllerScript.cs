using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public const int columns = 4;
    public const int rows = 2;

    public GameObject gamePnl;
    public GameObject endGamePnl;

    public const float Xspace = 400f;
    public const float Yspace = -400f;

    [SerializeField] public MainImageScript startObject;
    [SerializeField] public Sprite[] images;

    public int[] Randomiser(int[] locations)
    {
        int[] array = locations.Clone() as int[];
        for (int i = 0; i < array.Length; i++)
        {
            int newArray = array[i];
            int j = UnityEngine.Random.Range(i, array.Length);
            array[i] = array[j];
            array[j] = newArray;
        }
        return array;
    }

    public void Start()
    {
        int[] locations = { 0, 0, 1, 1, 2, 2, 3, 3 };
        locations = Randomiser(locations);

        Vector3 startPosition = startObject.transform.position;

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                MainImageScript gameImage;
                if (i == 0 && j == 0)
                {
                    gameImage = startObject;
                }
                else
                {
                    gameImage = Instantiate(startObject) as MainImageScript;
                }

                int index = j * columns + i;
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id]);

                float positionX = (Xspace * i) + startPosition.x;
                float positionY = (Yspace * j) + startPosition.y;

                gameImage.transform.position = new Vector3(positionX, positionY, startPosition.z);
            }
        }
    }

    public MainImageScript firstOpen;
    public MainImageScript secondOpen;

    private readonly ForgottenResults results = new();
    private readonly ResultsManager resultsManager = new();

    private int score = 0;
    private int attempts = 0;
    private float timeToFinish;

    [SerializeField] private TextMesh scoreText;
    [SerializeField] private TextMesh attemptsText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text attemptsResultsText;
    [SerializeField] private TMP_Text timeToFinishText;

    [SerializeField] private GameObject alertPnl;
    [SerializeField] private TMP_Text alertText;

    public bool canOpen
    {
        get { return secondOpen == null; }
    }

    public void imageOpened(MainImageScript startObject)
    {
        if (firstOpen == null)
        {
            firstOpen = startObject;
        }
        else
        {
            secondOpen = startObject;
            StartCoroutine(CheckGuessed());
        }
    }

    public IEnumerator CheckGuessed()
    {
        if (firstOpen.spriteId == secondOpen.spriteId) // Compares the two objects
        {
            score++; // Add score
            scoreText.text = "Score: " + score;
            if (score == 4)
            {
                gamePnl.SetActive(false);
                endGamePnl.SetActive(true);

                dateText.text = "Date: " + DateTime.Now.ToString("dd-MM-yyyy");
                attemptsResultsText.text = "Total attempts: " + attempts;
                timeToFinishText.text = "Total time needed to finish: " + timeToFinish.ToString("0.00") + " seconds";
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f); // Start timer

            firstOpen.Close();
            secondOpen.Close();
        }

        attempts++;
        attemptsText.text = "Attempts: " + attempts;

        firstOpen = null;
        secondOpen = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Update()
    {
        timeToFinish += Time.deltaTime;
    }

    public void SaveResults()
    {
        results.SetDate(DateTime.Now.ToString("dd-MM-yyyy"));
        results.SetAttempts(attempts);
        results.SetTimeToFinish(timeToFinish);

        resultsManager.SetForgottenResults(results);
        StartCoroutine(AddResult());
    }

    public IEnumerator AddResult()
    {
        Debug.Log(results.GetAttempts());
        string resultsToJson = JsonUtility.ToJson(results);
        Debug.Log(resultsToJson);
        ForgottenResults r = (ForgottenResults)JsonUtility.FromJson(resultsToJson, typeof(ForgottenResults));
        Debug.Log(r.GetAttempts());
        WWWForm form = new WWWForm();
        int id = DBManager.activePlayerId;
        form.AddField("id", id);
        form.AddField("results", resultsToJson);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/AddForgottenResult.php", form))
        {
            yield return www.SendWebRequest();

            if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
            {
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = "Results upload failed: " + www.error;
            }
            else
            {
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = www.downloadHandler.text;
            }
        }
    }
}