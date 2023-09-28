using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoadResults : MonoBehaviour
{
    public GameObject alertPnl;
    public TMP_Text alertText;
    private BeatsResults beatsResult;
    private DisplayResults displayResults;
    private ForgottenResults forgottenResult;
    private string id;
    private ResultsManager results;
    private List<ResultsManager> resultsManager;
    private SharperResults sharperResult;
    private int v_index, f_index, b_index, s_index;
    private VoiceResults voiceResult;

    //Starting the coroutine GetResults
    public void GetUserResults()
    {
        results = new ResultsManager();
        resultsManager = new List<ResultsManager>();
        StartCoroutine(GetResults());
    }

    //Dispplaying the next Beats results
    public void NextBeatsResult()
    {
        b_index++;
        if (b_index >= 0 && b_index < results.GetBeatsResults().Count)
        {
            displayResults.ShowBeatsResults(results, b_index);
        }
        else if (b_index == -1)
        {
            b_index = 0;
        }
        else if (b_index == results.GetBeatsResults().Count)
        {
            b_index = results.GetBeatsResults().Count - 1;
        }
    }

    //Dispplaying the next Forgotten results
    public void NextForgottenResult()
    {
        f_index++;
        if (f_index >= 0 && f_index < results.GetForgottenResults().Count)
        {
            displayResults.ShowForgottenResults(results, f_index);
        }
        else if (f_index == -1)
        {
            f_index = 0;
        }
        else if (f_index == results.GetForgottenResults().Count)
        {
            f_index = results.GetForgottenResults().Count - 1;
        }
    }

    //Dispplaying the next Sharper results
    public void NextSharperResult()
    {
        s_index++;
        if (s_index >= 0 && s_index < results.GetSharperResults().Count)
        {
            displayResults.ShowSharperResults(results, s_index);
        }
        else if (s_index == -1)
        {
            s_index = 0;
        }
        else if (s_index == results.GetSharperResults().Count)
        {
            s_index = results.GetSharperResults().Count - 1;
        }
    }

    //Dispplaying the next Voice results
    public void NextVoiceResult()
    {
        v_index++;
        if (v_index >= 0 && v_index < results.GetVoiceResults().Count)
        {
            displayResults.ShowVoiceResults(results, v_index);
        }
        else if (v_index == -1)
        {
            v_index = 0;
        }
        else if (v_index == results.GetVoiceResults().Count)
        {
            v_index = results.GetVoiceResults().Count - 1;
        }
    }

    //Dispplaying the previous Beats results
    public void PrevBeatsResult()
    {
        b_index--;
        if (b_index >= 0 && b_index < results.GetBeatsResults().Count)
        {
            displayResults.ShowBeatsResults(results, b_index);
        }
        else if (b_index == -1)
        {
            b_index = 0;
        }
        else if (b_index == results.GetBeatsResults().Count)
        {
            b_index = results.GetBeatsResults().Count - 1;
        }
    }

    //Dispplaying the previous Forgotten results
    public void PrevForgottenResult()
    {
        f_index--;
        if (f_index >= 0 && f_index < results.GetForgottenResults().Count)
        {
            displayResults.ShowForgottenResults(results, f_index);
        }
        else if (f_index == -1)
        {
            f_index = 0;
        }
        else if (f_index == results.GetForgottenResults().Count)
        {
            f_index = results.GetForgottenResults().Count - 1;
        }
    }

    //Dispplaying the previous Sharper results
    public void PrevSharperResult()
    {
        s_index--;
        if (s_index >= 0 && s_index < results.GetSharperResults().Count)
        {
            displayResults.ShowSharperResults(results, s_index);
        }
        else if (s_index == -1)
        {
            s_index = 0;
        }
        else if (s_index == results.GetSharperResults().Count)
        {
            s_index = results.GetSharperResults().Count - 1;
        }
    }

    //Dispplaying the previous Voice results
    public void PrevVoiceResult()
    {
        v_index--;
        if (v_index >= 0 && v_index < results.GetVoiceResults().Count)
        {
            displayResults.ShowVoiceResults(results, v_index);
        }
        else if (v_index == -1)
        {
            v_index = 0;
        }
        else if (v_index == results.GetVoiceResults().Count)
        {
            v_index = results.GetVoiceResults().Count - 1;
        }
    }

    //function called when clicking Beats results tab in the results panel
    public void ShowBeatsResults()
    {
        b_index = results.GetBeatsResults().Count - 1;
        displayResults.ShowBeatsResults(results, b_index);
    }

    //function called when clicking Forgotten results tab in the results panel
    public void ShowForgottenResults()
    {
        f_index = results.GetForgottenResults().Count - 1;
        displayResults.ShowForgottenResults(results, f_index);
    }

    //function called when clicking Sharper results tab in the results panel
    public void ShowSharperResults()
    {
        s_index = results.GetSharperResults().Count - 1;
        displayResults.ShowSharperResults(results, s_index);
    }

    //function called when clicking Voice results tab in the results panel
    public void ShowVoiceResults()
    {
        v_index = results.GetVoiceResults().Count - 1;
        displayResults.ShowVoiceResults(results, v_index);
    }

    //function called once upon object instantiation
    private void Awake()
    {
        displayResults = GetComponent<DisplayResults>();
        voiceResult = new VoiceResults();
        forgottenResult = new ForgottenResults();
        beatsResult = new BeatsResults();
        sharperResult = new SharperResults();
        v_index = 0;
        f_index = 0;
        b_index = 0;
        s_index = 0;
    }

    //Searching the DB for results associated with current player id and adding them to the ResultsManager.
    private IEnumerator GetResults()
    {
        resultsManager.Clear();
        id = DBManager.activePlayerId.ToString();
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/GetUserResults.php", form))
        {
            yield return www.SendWebRequest();

            //if erorrs are returned, display them to the user
            if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
            {
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = "Failed to retrive the results, Error: " + www.error;
            }
            //if no errors are found, attempt to split the returned string by tabs
            else
            {
                string content = www.downloadHandler.text;
                string[] splitContent = content.Split("\t");
                //if element returned at at index 0 is equal to 0, then display the error that was returned.
                if (splitContent[0] == "0")
                {
                    alertText.text = "";
                    alertPnl.SetActive(true);
                    alertText.text = splitContent[1];
                }
                //if element returned at index 0 is equal to 1, then for each element that is not equal to 1, split that element by * and deserialize de Json objects
                //and assign them to them to the matching object type, then place the objects into the ResultsManager object.
                //
                else if (splitContent[0] == "1")
                {
                    for (int i = 0; i < splitContent.Length - 1; i++)
                    {
                        if (splitContent[i] != "1")
                        {
                            string rowContent = splitContent[i];
                            string[] splitRowContent = rowContent.Split("*");
                            //deserialize the voice results from JSON to VoiceResults object found at position 0 in the array
                            voiceResult = (VoiceResults)JsonUtility.FromJson(splitRowContent[0], typeof(VoiceResults));
                            if (voiceResult != null)
                            {
                                results.SetVoiceResults(voiceResult);
                            }
                            //deserialize the forgotten results from JSON to ForgottenResults object found at position 1 in the array
                            forgottenResult = (ForgottenResults)JsonUtility.FromJson(splitRowContent[1], typeof(ForgottenResults));
                            if (forgottenResult != null)
                            {
                                results.SetForgottenResults(forgottenResult);
                            }
                            //deserialize the beats results from JSON to BeatsResults object found at position 2 in the array
                            beatsResult = (BeatsResults)JsonUtility.FromJson(splitRowContent[2], typeof(BeatsResults));
                            if (beatsResult != null)
                            {
                                results.SetBeatsResults(beatsResult);
                            }
                            //deserialize the sharper results from JSON to SharperResults object found at position 3 in the array
                            sharperResult = (SharperResults)JsonUtility.FromJson(splitRowContent[3], typeof(SharperResults));
                            if (results.GetSharperResults().Count == 0)
                            {
                                if (sharperResult != null)
                                {
                                    results.SetSharperResults(sharperResult);
                                }
                            }
                            else
                            {
                                if (sharperResult != null && (sharperResult.GetDate() != results.GetSharperResults()[^1].GetDate() || sharperResult.GetScore() != results.GetSharperResults()[^1].GetScore()))
                                {
                                    results.SetSharperResults(sharperResult);
                                }
                            }
                        }
                    }
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
}