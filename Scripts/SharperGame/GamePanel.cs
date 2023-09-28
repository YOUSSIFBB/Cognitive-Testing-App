using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{
    public TMP_Text scoreText, dateText, alertText;
    public GameObject alertPnl;
    public Score scoreScript;

    private readonly SharperResults results = new();
    private readonly ResultsManager resultsManager = new();

    private void OnEnable()
    {
        // Get the score from the Score script and display it in the panel
        scoreText.text = "Score: " + scoreScript.GetScore().ToString();
        dateText.text = "Date of test: " + DateTime.Now.ToString("dd-MM-yyyy");
    }

    public void OnRestartButtonClicked()
    {
        // Reload from game begining
        SceneManager.LoadScene(8);
        scoreScript.ResetScore();
    }

    public void OnMainMenuButtonClicked()
    {
        // Load the main menu scene
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        Cursor.visible = true;
    }

    public void SharperSaveResult()
    {
        //adding the game results to ResultsManager
        results.SetDate(DateTime.Now.ToString("dd-MM-yyyy"));
        results.SetScore(scoreScript.GetScore().ToString());
        resultsManager.SetSharperResults(results);
        //adding the game results to the DB
        StartCoroutine(AddSharperResult());
    }

    private IEnumerator AddSharperResult()
    {
        string resultsToJson = JsonUtility.ToJson(results);
        WWWForm form = new WWWForm();
        int id = DBManager.activePlayerId;
        form.AddField("id", id);
        form.AddField("results", resultsToJson);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/AddSharperResult.php", form))
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