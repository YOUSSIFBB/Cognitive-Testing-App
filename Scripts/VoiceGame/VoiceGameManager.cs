using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Random = UnityEngine.Random;

public class VoiceGameManager : MonoBehaviour
{
    public GameObject barHolder, resultsPnl, gamePnl, alertPnl;
    public Transform cardSlot;
    public List<Card> deck = new();
    public Image timerImage;
    private readonly string[] keyWords = { "apple", "snake", "plane", "slide", "smoothie", "balloons", "blanket", "book", "brush", "chair", "clock", "stove", "crib", "spoon", "watch", "train", "tootbrush", "thumb", "swing", "sun", "yogurt", "zebra", "dishes", "dresser", "feather", "flag", "frog", "giraffe", "glue", "guitar", "hammer", "vacuum", "ketchup", "ladder", "mop", "teeth", "muffin", "orange", "pajamas", "piano", "pretzel", "rabbit", "scissors", "shovel", "skunk" };

    private readonly float maxTime = 5.0f, transitionSpeed = 100;
    private readonly List<string> missedWords = new();
    private VoiceResults results = new();
    private readonly ResultsManager resultsManager = new();
    private string activeCard, activeCardGroup;

    [SerializeField]
    private TMP_Text alertText, barText, cardText, scoreText, missedWordsText, dateText, playerText, avgTimeToRespondText;

    private KeywordRecognizer m_Recognizer;

    private bool newGame = true, isPaused = true;
    private int score, matchedWordsCount;
    private float timeRemaining, displayScore, timeLeft, timeToRespond;

    public UnityWebRequest testResult;
    //method to draw a card
    public void DrawCard()
    {
        if (deck.Count >= 1)
        {
            if (activeCard != null)
            {
                GameObject.Find(activeCard).SetActive(false);
            }

            Card randCard = deck[Random.Range(0, deck.Count)];
            randCard.gameObject.SetActive(true);
            randCard.transform.position = cardSlot.position;
            deck.Remove(randCard);
            activeCard = randCard.name;
            activeCardGroup = randCard.group;
            cardText.text = randCard.name;
            timeRemaining = maxTime;
            return;
        }
        else
        {
            PauseGame();
            EndGame();
        }
    }

    //saving the results
    public void SaveResults()
    {
        //adding the game results to ResultsManager
        results.SetDate(DateTime.Now.ToString("dd-MM-yyyy"));
        results.SetTimeToRespond(timeToRespond);
        resultsManager.SetVoiceResults(results);

        //adding the game results to the DB
        StartCoroutine(AddResult());
    }

    //starting the key word recognizer
    public void StartRec()
    {
        if (newGame)
        {
            newGame = false;
            barHolder.SetActive(true);
            scoreText.gameObject.SetActive(true);
            isPaused = false;
            m_Recognizer = new KeywordRecognizer(keyWords);
            m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
            m_Recognizer.Start();
        }
        else if (!m_Recognizer.IsRunning)
        {
            m_Recognizer.Start();
            isPaused = false;
        }
    }

    //method to update the score shown to the player
    public void UpdateScoreDisplay()
    {
        scoreText.text = string.Format("Score: {0:000}", displayScore);
    }

    //method to add the medical results to the database
    private IEnumerator AddResult()
    {
        string resultsToJson = JsonUtility.ToJson(results);
        WWWForm form = new WWWForm();
        int id = DBManager.activePlayerId;
        form.AddField("id", id);
        form.AddField("results", resultsToJson);
        using UnityWebRequest www = UnityWebRequest.Post("http://localhost/MindMatters/AddVoiceResult.php", form);

        testResult = www;
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

    //method to define the end of the game and activate the end of game results panel
    private void EndGame()
    {
        gamePnl.SetActive(false);
        resultsPnl.SetActive(true);

        var avgTimeLeft = timeLeft / matchedWordsCount;
        timeToRespond = 5 - avgTimeLeft;
        avgTimeToRespondText.text = timeToRespond.ToString("0.00");

        playerText.text = DBManager.activePlayerName;

        dateText.text = DateTime.Now.ToString("dd-MM-yyyy");

        var counts = missedWords.GroupBy(i => i);
        foreach (var group in counts)
        {
            results.AddMissedSound(group.Count(), group.Key);
        }

        string missedWordsTextString = "";
        foreach (var group in results.GetMissedSounds())
        {
            missedWordsTextString += "The " + group.sound + " was missed " + group.value + " times.\n";
        }
        missedWordsText.text = missedWordsTextString;
    }

    //method to recognize the phrase spoken and act accordingly
    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.text == activeCard.ToLower())
        {
            //score shown to the player
            barText.text = "Correct!";
            barText.gameObject.SetActive(true);
            score += 10;
            UpdateScoreDisplay();

            //medical score
            timeLeft = timeLeft + timeRemaining;
            matchedWordsCount++;
        }
        else if (activeCardGroup != null)
        {
            barText.text = "Wrong Keyword!";
            barText.gameObject.SetActive(true);
            score -= 5;
            UpdateScoreDisplay();
            missedWords.Add(activeCardGroup);
        }
        DrawCard();
    }

    //method to pause the game
    private void PauseGame()
    {
        isPaused = true;
        m_Recognizer.Stop();
    }

    //method called once every frame
    private void Update()
    {
        if (!isPaused)
        {
            UpdateTime();
            displayScore = Mathf.MoveTowards(displayScore, score, transitionSpeed * Time.deltaTime);
            UpdateScoreDisplay();
        }
    }

    //method that animates the timer bar, prompts the player if a word was corectly spoken and calls DrawCard if the time runs out
    private void UpdateTime()
    {
        if (timeRemaining > 0)
        {
            //modifying the size of the filer image of the timer bar based on the time left
            timeRemaining -= Time.deltaTime;
            timerImage.fillAmount = timeRemaining / maxTime;
        }
        else
        {
            if (activeCardGroup != null)
            {
                barText.text = "Time Out!";
                barText.gameObject.SetActive(true);
                score -= 5;
                UpdateScoreDisplay();
                missedWords.Add(activeCardGroup);
            }
            DrawCard();
        }
        if (timeRemaining < 3)
        {
            barText.gameObject.SetActive(false);
        }
    }

    //methods added for testing purpeses
    public string GetScoreText()
    {
        return scoreText.text;
    }

    public KeywordRecognizer GetKeywordRecognizer()
    {
        return m_Recognizer;
    }

    public void SetMissedWords(string value)
    {
        missedWords.Add(value);
    }

    public void EndGameTest()
    {
        EndGame();
    }

    public VoiceResults GetVoiceResults()
    {
        return results;
    }

    public void SetScore(int value)
    {
        score = value;
    }

    public void UpdateTest()
    {
        Update();
    }

    public void SetIsPaused(bool value)
    {
        isPaused = value;
    }

    public bool GetNewGame()
    {
        return newGame;
    }

    public void PauseGameTest()
    {
        PauseGame();
    }

    public void SetVoiceResults(VoiceResults value)
    {
        results = value;
    }

    public void SetID(string value)
    {
        DBManager.activeUser_id = value;
    }

    public UnityWebRequest GetWWWResult()
    {
        return testResult;
    }

    public void SetTimeRemaining(float value)
    {
        timeRemaining = value;
    }

    public bool GetBarTextObjState()
    {
        return barText.gameObject.activeSelf;
    }

    public void SetBarTextBojState(bool value)
    {
        barText.gameObject.SetActive(value);
    }
    public void SetActiveCardGroup(string value)
    {
        activeCardGroup = value;
    }
}