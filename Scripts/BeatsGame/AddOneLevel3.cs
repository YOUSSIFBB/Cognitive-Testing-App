using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddOneLevel3 : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    private static int number;
    public AudioClip[] sounds;
    private AudioSource source;
    public GameObject beatsLvl3GamePnl;
    private bool clickedOnce;

    [Range(0.1f, 0.7f)]                         //value range header for volume
    public float volumeChangeMultiplyer = 0.4f;

    [Range(0.1f, 0.10f)]                        //value range header for pitch
    public float pitchMultiplyer = 0.4f;

    //Lists to store the missed buttons, volume, and pitch variables for the level 3
    private BeatsResultsLvl3 beatsMissedSound;

    private List<BeatsResultsLvl3> beatsMissedSoundList;

    private void Awake()
    {
        //get the audio source
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);
        number = 0;
        beatsMissedSoundList = new List<BeatsResultsLvl3>();
    }

    // Coroutine to play a sound every 5 seconds
    //Reference: https://stackoverflow.com/questions/68889731/difference-between-whiletrue-in-coroutine-then-put-it-in-the-void-start-and/68889924#68889924
    private IEnumerator PlaySoundCoroutine()
    {
        if (sounds.Length > 0)
        {
            clickedOnce = false;
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip soundToPlay = sounds[randomIndex];

            beatsMissedSound = new BeatsResultsLvl3();

            //prevent the sounds from overlapping when another audio is playing
            if (source.isPlaying)
            {
                source.Stop();
            }

            source.volume = Random.Range(1 - volumeChangeMultiplyer, 1);
            source.pitch = Random.Range(1 - pitchMultiplyer, 1 + pitchMultiplyer);

            //there is 2 sheep sounds and 2 horse sounds in the array, we can change the direction of the audio by specifying the index of the sound and playing either to the left or to the right
            if (randomIndex == 0)
            {
                source.panStereo = 1; //play the first sheep soudn to the Left
                beatsMissedSound.SetSide("Right");
            }
            else if (randomIndex == 1)
            {
                source.panStereo = -1; //play the second sheep sound to the Right
                beatsMissedSound.SetSide("Left");
            }
            else if (randomIndex == 2)
            {
                source.panStereo = 1; //play the first horse sound to the Left
                beatsMissedSound.SetSide("Right");
            }
            else if (randomIndex == 3)
            {
                source.panStereo = -1; //play the second horse sound to the Right
                beatsMissedSound.SetSide("Left");
            }

            source.PlayOneShot(soundToPlay);

            beatsMissedSound.SetPitchLvl3(source.pitch);
            beatsMissedSound.SetVolumeLvl3(source.volume);
            beatsMissedSound.SetMissedSoundLvl3(sounds[randomIndex].name);
        }
        yield return new WaitForSeconds(2f);            //wait for two seconds before playing a sound again

        //score limit to swtitch scenes
        if (number == 10)
        {
            yield break;            //stop playing the sound when the score reaches 10
        }
    }

    //buttons logic for score increment
    public void ButtonClicked()
    {
        if (number >= 10)
        {
            SceneManager.LoadScene("GameOver");  //name of Results scene
        }
        var clickedbtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (clickedbtn.name == (beatsMissedSound.GetMissedSoundLvl3() + "Button" + beatsMissedSound.GetSide()) && number < 10 && clickedOnce == false && !source.isPlaying)
        {
            clickedOnce = true;
            number++;
            numberText.text = number.ToString();
            StartCoroutine(PlaySoundCoroutine()); //call method for IEnumerator
        }
        else if (number < 10 && clickedOnce == false && !source.isPlaying)
        {
            clickedOnce = true;
            beatsMissedSoundList.Add(beatsMissedSound);
            StartCoroutine(PlaySoundCoroutine()); //call method for IEnumerator
        }
    }

    public void BeatsPlayLvl3()
    {
        StartCoroutine(PlaySoundCoroutine()); //call method for IEnumerator
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.SetActive(false);
        beatsLvl3GamePnl.SetActive(true);
    }

    //get methods for the MissedButtons, rVolumes, and rPitches for level 3
    public List<BeatsResultsLvl3> GetBeatsResultsLvl3()
    {
        return beatsMissedSoundList;
    }

    public void ClearList()
    {
        beatsMissedSoundList.Clear();
    }
}