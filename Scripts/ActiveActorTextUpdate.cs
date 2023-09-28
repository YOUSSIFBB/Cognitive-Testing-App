using TMPro;
using UnityEngine;

public class ActiveActorTextUpdate : MonoBehaviour
{
    public TMP_Text playerText;
    public TMP_Text userText;

    //method called when object is intstantiated, it will check if the user or player are logged in and display their name
    private void Start()
    {
        if (DBManager.LoggedIn)
        {
            userText.text = "Active User: " + DBManager.activeUsername;
        }
        if (DBManager.PlayerActive)
        {
            playerText.text = "Active Player: " + DBManager.activePlayerName;
        }
    }
}