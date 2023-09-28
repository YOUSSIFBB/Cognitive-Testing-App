using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSearch : MonoBehaviour
{
    public GameObject alertPnl;
    public TMP_Text alertText;
    public TMP_InputField nameInputField;
    public Button searchBtn;
    public Button selectBtn;

    private ActiveActorTextUpdate activeActorTextUpdate;
    private string nameCell, surnameCell, birthYrCell, ppsNoCell;
    private Player player;
    private List<Player> playerList = new List<Player>();
    private List<Transform> playerTransformList;
    private int rowBackgroundTracker;
    private Transform tableRow;
    private Transform tableRowContainer;

    //Starting the coroutine SearchPlayer
    public void CallSearchPlayer()
    {
        StartCoroutine(SearchPlayer());
    }

    //method to select a player and set it as active
    public void SetPlayerActive()
    {
        if (nameCell != null)
        {
            foreach (Player p in playerList)
            {
                if (p.GetPpsNo() == ppsNoCell)
                {
                    DBManager.activePlayerId = p.GetId();
                    DBManager.activePlayerName = nameCell;
                    DBManager.activePlayerSurname = surnameCell;
                    DBManager.activePlayerBirthYr = int.Parse(birthYrCell);
                    DBManager.activePlayerPpsNo = ppsNoCell;
                }
            }

            activeActorTextUpdate = GameObject.FindObjectOfType<ActiveActorTextUpdate>();
            activeActorTextUpdate.playerText.text = "Active Player: " + nameCell;

            GameObject.Find("PlayerNameLabel").GetComponent<TMP_Text>().text = "Name: " + nameCell;
            GameObject.Find("PlayerSurnameLabel").GetComponent<TMP_Text>().text = "Surname:       " + surnameCell;
            GameObject.Find("PlayerAgeLabel").GetComponent<TMP_Text>().text = "Age:    " + (2023 - int.Parse(birthYrCell));
            GameObject.Find("PlayerPpsNoLabel").GetComponent<TMP_Text>().text = "PPS Number: " + ppsNoCell;
        }
        selectBtn.interactable = false;
    }

    //temporary save the data found in the clicked row
    public void TableRowBtn()
    {
        selectBtn.interactable = true;

        var clickedbtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        nameCell = clickedbtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text;

        surnameCell = clickedbtn.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text;

        birthYrCell = clickedbtn.gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text;

        ppsNoCell = clickedbtn.gameObject.transform.GetChild(4).GetComponent<TMP_Text>().text;
    }

    //method to make the search button interactible only after the search input field has valid values
    public void VerifyInputs()
    {
        searchBtn.interactable = (nameInputField.text.Length >= 1);
    }

    //adding new rows for each element found in the playerList.
    private void AddToTable()
    {
        rowBackgroundTracker = 0;
        tableRowContainer = transform.Find("TableRowContainer");
        tableRow = tableRowContainer.Find("TableRow");

        foreach (GameObject clone in GameObject.FindGameObjectsWithTag("Selectable"))
        {
            if (clone.name == "TableRow(Clone)")
            {
                Destroy(clone);
            }
        }

        tableRow.gameObject.SetActive(false);
        playerTransformList = new List<Transform>();
        foreach (Player player in playerList)
        {
            CreatePlayerEntryTransform(player, tableRowContainer, playerTransformList);

            rowBackgroundTracker++;
        }
    }

    //method to add a new row to the found players table
    private void CreatePlayerEntryTransform(Player player, Transform container, List<Transform> transformList)
    {
        float rowHeight = 40f;

        Transform entryTransform = Instantiate(tableRow, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -rowHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        string name = player.GetName();
        entryTransform.Find("NameTblRowLabel").GetComponent<TMP_Text>().text = name;
        string surname = player.GetSurname();
        entryTransform.Find("SurnameTblRowLabel").GetComponent<TMP_Text>().text = surname;
        int birthYr = player.GetBirthYr();
        entryTransform.Find("BirthYrTblRowLabel").GetComponent<TMP_Text>().text = birthYr.ToString();
        string ppsNo = player.GetPpsNo();
        entryTransform.Find("PPSNoTblRowLabel").GetComponent<TMP_Text>().text = ppsNo;

        entryTransform.Find("rowBackground").gameObject.SetActive(rowBackgroundTracker % 2 == 1);
        transformList.Add(entryTransform);
    }

    //Searching the DB for players and adding them to the playerList.
    private IEnumerator SearchPlayer()
    {
        playerList.Clear();
        WWWForm form = new WWWForm();
        form.AddField("name", nameInputField.text);
        form.AddField("user_id", int.Parse(DBManager.activeUser_id));
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/SearchPlayer.php", form))
        {
            yield return www.SendWebRequest();

            //if erorrs are returned, display them to the user
            if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
            {
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = "User login failed: " + www.error;
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
                //if element returned at index 0 is equal to 1, then for each element that is not equal to 1, split that element by * and assign the data to the Player oject
                else if (splitContent[0] == "1")
                {
                    rowBackgroundTracker = 0;
                    for (int i = 0; i < splitContent.Length - 1; i++)
                    {
                        if (splitContent[i] != "1")
                        {
                            string rowContent = splitContent[i];
                            string[] splitRowContent = rowContent.Split("*");

                            player = new Player();

                            player.SetId(int.Parse(splitRowContent[0]));

                            player.SetName(splitRowContent[1]);

                            player.SetSurname(splitRowContent[2]);

                            player.SetBirthYr(int.Parse(splitRowContent[3]));

                            player.SetPpsNo(splitRowContent[4]);
                            playerList.Add(player);
                        }
                    }

                    AddToTable();
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