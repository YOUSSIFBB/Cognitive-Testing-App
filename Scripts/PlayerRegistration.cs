using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerRegistration : MonoBehaviour
{
    public Button addPlayer;
    public GameObject alertPnl;
    public TMP_Text alertText;
    public TMP_InputField birthYrField, nameField, ppsNoField, sNameField;
    private ActiveActorTextUpdate activeActorTextUpdate;

    public void CallAddPlayer()
    {
        StartCoroutine(AddPlayer());
    }

    //method to make the addPlayer button interactible only if the inputs are valid
    public void VerifyInputs()
    {
        //checking if the inuted birth year is a valid 
        if (int.TryParse(birthYrField.text, out _))
        {
            int birthYr = int.Parse(birthYrField.text);
            int currYear = DateTime.Now.Year;
            bool validBYear = (birthYr < currYear && birthYr > 1930);

            Regex RgxUrl = new("^[0-9]{7}[a-zA-Z]{2}$");
            bool validPPSNo = RgxUrl.IsMatch(ppsNoField.text);

            RgxUrl = new("^[a-zA-Z]{3,10}$");
            bool validName = RgxUrl.IsMatch(nameField.text);

            RgxUrl = new("^[a-zA-Z]{3,10}$");
            bool validSurname = RgxUrl.IsMatch(sNameField.text);

            addPlayer.interactable = (validPPSNo && validName && validSurname && validBYear);
        }
    }

    //Method to add a player
    private IEnumerator AddPlayer()
    {
        int birthYr = int.Parse(birthYrField.text);
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("sName", sNameField.text);
        form.AddField("birthYr", birthYr);
        form.AddField("ppsNo", ppsNoField.text);
        form.AddField("user_id", DBManager.activeUser_id);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/RegisterPlayer.php", form))
        {
            yield return www.SendWebRequest();

            if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
            {
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = "Player registration failed: " + www.error;
            }
            else
            {
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = www.downloadHandler.text;
                DBManager.activePlayerName = nameField.text;
                activeActorTextUpdate = GameObject.FindObjectOfType<ActiveActorTextUpdate>();
                activeActorTextUpdate.playerText.text = "Active Player: " + nameField.text;
            }
        }
    }
}