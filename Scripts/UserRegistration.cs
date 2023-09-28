using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserRegistration : MonoBehaviour
{
    public GameObject alertPnl;
    public TMP_Text alertText;
    public TMP_InputField emailField, keyField, nameField, passwordField;
    public Button submitButton;

    private bool validEmail, validPassword;
    private Regex RgxUrl;

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    //making the submit button interactible once the inputs are valid
    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && validPassword && validEmail);
    }

    public void VerifyKey()
    {
        if (keyField.text.Length == 4)
        {
            GameObject.Find("KeyFeedBackText").GetComponent<TMP_Text>().text = "Please provide the activation key provided with your purchase.";
        }
        else { GameObject.Find("KeyFeedBackText").GetComponent<TMP_Text>().text = ""; }
    }

    public void VerifyUserName()
    {
        if (nameField.text.Length < 8)
        {
            GameObject.Find("UsernameFeedBackText").GetComponent<TMP_Text>().text = "The username must have at least 8 characters.";
        }
        else { GameObject.Find("UsernameFeedBackText").GetComponent<TMP_Text>().text = ""; }
    }

    public void VerifyEmail()
    {
        //checking for @ and . characters in the email input
        RgxUrl = new("[@]");
        bool atCharacter = RgxUrl.IsMatch(emailField.text);
        RgxUrl = new("[.]");
        bool dotCharacter = RgxUrl.IsMatch(emailField.text);
        validEmail = atCharacter && dotCharacter && emailField.text.Length > 5;

        if (!validEmail)
        {
            GameObject.Find("EmailFeedBackText").GetComponent<TMP_Text>().text = "Please enter a valid email in the form of example@example.something";
        }
        else { GameObject.Find("EmailFeedBackText").GetComponent<TMP_Text>().text = ""; }
    }

    public void VerifyPassword()
    {
        //checking if the pasword  input contains special characters letters and numbers
        RgxUrl = new("[@!#$%&'*+/=?^_`{|}~-]");
        bool passswordSpecialChar = RgxUrl.IsMatch(passwordField.text);
        RgxUrl = new("[a-z]");
        bool passwordLowerCaseLetter = RgxUrl.IsMatch(passwordField.text);
        RgxUrl = new("[A-Z]");
        bool passwordUpperCaseLetter = RgxUrl.IsMatch(passwordField.text);
        RgxUrl = new("[0-9]");
        bool passwordNumber = RgxUrl.IsMatch(passwordField.text);
        validPassword = passswordSpecialChar && passwordLowerCaseLetter && passwordUpperCaseLetter && passwordNumber && passwordField.text.Length >= 8;

        string pwdFeedBackText = "The password must include at least:\n";
        if (!passswordSpecialChar)
        {
            pwdFeedBackText += "One special character: @!#$%&'*+/=?^_`{|}~-\n";
        }
        else if (!passwordLowerCaseLetter)
        {
            pwdFeedBackText += "One lowercase letter\n";
        }
        else if (!passwordUpperCaseLetter)
        {
            pwdFeedBackText += "One uppercase letter\n";
        }
        else if (!passwordNumber)
        {
            pwdFeedBackText += "One number";
        }
        else if (passwordField.text.Length < 9 && passswordSpecialChar && passwordLowerCaseLetter && passwordUpperCaseLetter && passwordNumber)
        {
            pwdFeedBackText = "The password must have at least 8 characters.";
        }
        else { pwdFeedBackText = ""; }
        GameObject.Find("PasswordFeedBackText").GetComponent<TMP_Text>().text = pwdFeedBackText;
    }

    //submitting the data to a form then post is to the php file
    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        form.AddField("email", emailField.text);
        form.AddField("activationKey", keyField.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/RegisterUser.php", form))
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
                alertText.text = "";
                alertPnl.SetActive(true);
                alertText.text = www.downloadHandler.text;
            }
        }
    }
}