using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BeatsEndGameResults : MonoBehaviour
{
    //reference instances for classes AddOneLevel2 and AddOneLevel3
    private AddOneLevel2 level2Script;

    private AddOneLevel3 level3Script;
    private readonly BeatsResults results = new();
    private readonly ResultsManager resultsManager = new();

    //variables for the GUI Textbox display for game over screen
    public TMP_Text lvl2Results, lvl3Results, alertText;

    public GameObject alertPnl;

    private void Awake()
    {
        GameObject gameObject = GameObject.Find("AudioSourceLvl2");
        if (gameObject != null)
        {
            level2Script = gameObject.GetComponent<AddOneLevel2>();
        }
        gameObject = GameObject.Find("AudioSourceLvl3");
        if (gameObject != null)
        {
            level3Script = gameObject.GetComponent<AddOneLevel3>();
        }
        PrintResults();
    }

    public void PrintResults()
    {
        string lvl2Text = "";
        if (level2Script != null)
        {
            foreach (BeatsResultsLvl2 br in level2Script.GetBeatsResultsLvl2())
            {
                lvl2Text += "The " + br.GetMissedSoundLvl2() + " sound was missed at " + br.GetVolumeLvl2().ToString("0.00") + " volume, with a pitch of " + br.GetPitchLvl2().ToString("0.00") + " percent.\n";
            }
        }
        lvl2Results.text = lvl2Text;
        string lvl3Text = "";
        if (level3Script != null)
        {
            foreach (BeatsResultsLvl3 br in level3Script.GetBeatsResultsLvl3())
            {
                lvl3Text += "The " + br.GetMissedSoundLvl3() + " sound was missed at " + br.GetVolumeLvl3().ToString("0.00") + " volume, with a pitch of " + br.GetPitchLvl3().ToString("0.00") + ", on " + br.GetSide().ToLower() + " side.\n";
            }
        }
        lvl3Results.text = lvl3Text;
    }

    public void SaveResults()
    {
        //adding the game results to the DB
        StartCoroutine(AddResult());
    }

    public void BeatsResultsMenu()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name == "PlayAgainLvl1")
        {
            SceneManager.LoadScene(3);
        }
        else if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name == "PlayAgainLvl2")
        {
            SceneManager.LoadScene(4);
            level2Script.ClearList();
        }
        else if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name == "PlayAgainLvl3")
        {
            SceneManager.LoadScene(5);
            level3Script.ClearList();
        }
        else if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name == "BackToMainMenuBtn")
        {
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator AddResult()
    {
        //adding the game results to ResultsManager
        if (level2Script != null)
        {
            foreach (BeatsResultsLvl2 br in level2Script.GetBeatsResultsLvl2())
            {
                results.SetBeatsResultsLvl2(br);
            }
        }
        if (level3Script != null)
        {
            foreach (BeatsResultsLvl3 br in level3Script.GetBeatsResultsLvl3())
            {
                results.SetBeatsResultsLvl3(br);
            }
        }
        results.SetDate(DateTime.Now.ToString("dd-MM-yyyy"));
        resultsManager.SetBeatsResults(results);

        string resultsToJson = JsonUtility.ToJson(results);
        WWWForm form = new WWWForm();
        int id = DBManager.activePlayerId;
        form.AddField("id", id);
        form.AddField("results", resultsToJson);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/AddBeatsResult.php", form))
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