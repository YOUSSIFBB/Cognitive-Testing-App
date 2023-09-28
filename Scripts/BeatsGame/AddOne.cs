using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddOne : MonoBehaviour
{
    public GameObject startPnl;
    public TextMeshProUGUI numberText;
    private static int number = 0;         //make this var static s then the number is not reset after each click
    public AudioClip[] sounds;    //array of audio clip objects
    public AudioSource source;    //array of aduio source object that allow us to play the sounds we store in the array
    public string soundName;
    private bool clickedOnce;

    public void Start()
    {
        // get audio source
        source = GetComponent<AudioSource>();
    }

    //hide the play button when clicked on
    public void PlayButton()
    {
        startPnl.SetActive(false);
        StartCoroutine(PlaySoundCoroutine());
    }

    // Coroutine to play a sound every 5 seconds
    //Reference: https://stackoverflow.com/questions/68889731/difference-between-whiletrue-in-coroutine-then-put-it-in-the-void-start-and/68889924#68889924
    public IEnumerator PlaySoundCoroutine()
    {
        soundName = "";
        if (sounds.Length > 0)
        {
            clickedOnce = false;
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip soundToPlay = sounds[randomIndex];

            //prevent the sounds from overlapping when another audio is playing
            if (source.isPlaying)
            {
                source.Stop();
            }
            soundName = sounds[randomIndex].name;
            Debug.Log(soundName);
            source.PlayOneShot(soundToPlay);
            
        }
        yield return new WaitForSeconds(2f);                //wait for two seconds before playing a sound again

        //score limit to swtitch scenes
        if (number == 10)
        {
            yield break;
        }
    }

    //buttons logic for score increment
    public void BeatsBtnPressed()
    {
        if (number >= 10)
        {
            SceneManager.LoadScene("BeatsGameLevel2");  //name of level 2 scene
        }
        var clickedbtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedbtn.name == (soundName + "Button") && number < 10 && clickedOnce == false && !source.isPlaying)
        {
            clickedOnce = true;
            number++;
            numberText.text = number.ToString();
            StartCoroutine(PlaySoundCoroutine());
        }
        else if (number < 10 && clickedOnce == false && !source.isPlaying)
        {
            clickedOnce = true;
            StartCoroutine(PlaySoundCoroutine());
        }
    }


    //this is only for testing purposes, added just for testing
    public int GetSoundsArraySize(){
        return sounds.Length;
    }

    public bool GetStratPln(){
        return startPnl;
    }

    public string GetSoundName(){
        return soundName;
    }

    public bool GetButtonClickedOnce(){
        return clickedOnce;
    }

    
}