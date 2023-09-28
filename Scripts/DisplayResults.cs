using TMPro;
using UnityEngine;

public class DisplayResults : MonoBehaviour
{
    public GameObject alertPnl;
    public TMP_Text alertText;

    private VoiceResults voiceResult;
    private ForgottenResults forgottenResult;
    private BeatsResults beatsResult;
    private SharperResults sharperResult;

    //method to display Voice results
    public void ShowVoiceResults(ResultsManager inResults, int index)
    {
        GameObject.Find("v_DateText").GetComponent<TMP_Text>().text = "Date of test: ";
        GameObject.Find("v_AvgTimeToResponseText").GetComponent<TMP_Text>().text = "Average time to response: ";
        GameObject.Find("v_MissedSoundsText").GetComponent<TMP_Text>().text = "";
        if (inResults.GetVoiceResults().Count > 0)
        {
            if (index >= 0 && index < inResults.GetVoiceResults().Count)
            {
                voiceResult = inResults.GetVoiceResults()[index];
                GameObject.Find("v_DateText").GetComponent<TMP_Text>().text = "Date of test: " + voiceResult.GetDate();
                GameObject.Find("v_AvgTimeToResponseText").GetComponent<TMP_Text>().text = "Average time to response: " + voiceResult.GetTimeToRespond().ToString("0.00") + " seconds";
                string missedWordsTextString = "";
                foreach (var group in voiceResult.GetMissedSounds())
                {
                    missedWordsTextString += "The " + group.sound + " was missed " + group.value + " times.\n";
                }
                GameObject.Find("v_MissedSoundsText").GetComponent<TMP_Text>().text = missedWordsTextString;
            }
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No results added to the database yet.";
        }
    }

    //method to display Forgotten results
    public void ShowForgottenResults(ResultsManager inResults, int index)
    {
        GameObject.Find("f_DateText").GetComponent<TMP_Text>().text = "Date of test: ";
        GameObject.Find("f_AttemptsText").GetComponent<TMP_Text>().text = "Attempts: ";
        GameObject.Find("f_TimeToFinishText").GetComponent<TMP_Text>().text = "Time to finish: ";

        if (inResults.GetForgottenResults().Count > 0)
        {
            if (index >= 0 && index < inResults.GetForgottenResults().Count)
            {
                forgottenResult = inResults.GetForgottenResults()[index];
                GameObject.Find("f_DateText").GetComponent<TMP_Text>().text = "Date of test: " + forgottenResult.GetDate();
                GameObject.Find("f_AttemptsText").GetComponent<TMP_Text>().text = "Attempts: " + forgottenResult.GetAttempts();
                GameObject.Find("f_TimeToFinishText").GetComponent<TMP_Text>().text = "Time to finish: " + forgottenResult.GetTimeToFinish().ToString("0.00") + " seconds";
            }
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No results added to the database yet.";
        }
    }

    //method to display Beats results
    public void ShowBeatsResults(ResultsManager inResults, int index)
    {
        GameObject.Find("b_DateText").GetComponent<TMP_Text>().text = "Date of test: ";
        GameObject.Find("b_Lvl2ResultText").GetComponent<TMP_Text>().text = "";
        GameObject.Find("b_Lvl3ResultText").GetComponent<TMP_Text>().text = "";

        if (inResults.GetBeatsResults().Count > 0)
        {
            if (index >= 0 && index < inResults.GetBeatsResults().Count)
            {
                beatsResult = inResults.GetBeatsResults()[index];
                GameObject.Find("b_DateText").GetComponent<TMP_Text>().text = "Date of test: " + beatsResult.GetDate();

                string lvl2Text = "";
                if (beatsResult.GetBeatsResultsLvl2() != null)
                {
                    foreach (BeatsResultsLvl2 br in beatsResult.GetBeatsResultsLvl2())
                    {
                        lvl2Text += "The " + br.GetMissedSoundLvl2() + " sound was missed at " + br.GetVolumeLvl2().ToString("0.00") + " volume, with a pitch of " + br.GetPitchLvl2().ToString("0.00") + " percent.\n";
                    }
                }
                GameObject.Find("b_Lvl2ResultText").GetComponent<TMP_Text>().text = lvl2Text;

                string lvl3Text = "";
                if (beatsResult.GetBeatsResultsLvl3() != null)
                {
                    foreach (BeatsResultsLvl3 br in beatsResult.GetBeatsResultsLvl3())
                    {
                        lvl3Text += "The " + br.GetMissedSoundLvl3() + " sound was missed at " + br.GetVolumeLvl3().ToString("0.00") + " volume, with a pitch of " + br.GetPitchLvl3().ToString("0.00") + ", on " + br.GetSide().ToLower() + " side.\n";
                    }
                }
                GameObject.Find("b_Lvl3ResultText").GetComponent<TMP_Text>().text = lvl3Text;
            }
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No results added to the database yet.";
        }
    }

    //method to display Sharper results
    public void ShowSharperResults(ResultsManager inResults, int index)
    {
        GameObject.Find("s_DateText").GetComponent<TMP_Text>().text = "Date of test: ";
        GameObject.Find("s_ScoreText").GetComponent<TMP_Text>().text = "Score: ";

        if (inResults.GetSharperResults().Count > 0)
        {
            if (index >= 0 && index < inResults.GetSharperResults().Count)
            {
                sharperResult = inResults.GetSharperResults()[index];
                GameObject.Find("s_DateText").GetComponent<TMP_Text>().text = "Date of test: " + sharperResult.GetDate();
                GameObject.Find("s_ScoreText").GetComponent<TMP_Text>().text = "Score: " + sharperResult.GetScore();
            }
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No results added to the database yet.";
        }
    }
}