using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public GameObject alertPnl;
    public TMP_Text alertText;
    public GameObject Panel;

    public void GoBeatsGame()
    {
        if (DBManager.PlayerActive)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No Player selected!";
        }
    }

    public void GoSharperGame()
    {
        if (DBManager.PlayerActive)
        {
            SceneManager.LoadScene(8);
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No Player selected!";
        }
    }

    public void GoForgottenGame()
    {
        if (DBManager.PlayerActive)
        {
            SceneManager.LoadScene(7);
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No Player selected!";
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToVoiceGame()
    {
        if (DBManager.PlayerActive)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No Player selected!";
        }
    }

    public void HidePanel()
    {
        Panel.SetActive(false);
    }

    public void LogOut()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowAndDontHidePanel()
    {
        Panel.SetActive(true);
    }

    public void ShowPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    public void ShowResultsPnl()
    {
        Panel.SetActive(true);
        if (!DBManager.PlayerActive)
        {
            alertText.text = "";
            alertPnl.SetActive(true);
            alertText.text = "No Player selected!";
        }
    }
}