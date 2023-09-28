using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInScript : MonoBehaviour
{
    public GameObject alertPnl;
    public TMP_Text alertText;
    public TMP_InputField nameField, passwordField;
    public Button submitButton;

    public void CallLogin()
    {
        StartCoroutine(LoginUser());
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 2 && passwordField.text.Length >= 8);
    }

    private IEnumerator LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/login.php", form))
        {
            yield return www.SendWebRequest();

            if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
            {
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = "User login failed: " + www.error;
            }
            else
            {
                string content = www.downloadHandler.text;
                string sep = "\t";
                string[] splitContent = content.Split(sep.ToCharArray());

                if (splitContent[0] == "0")
                {
                    DBManager.activeUsername = nameField.text;
                    DBManager.activeUser_id = splitContent[1];
                    GoToMainMenu();
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