using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddOneLevel2 : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    private int number;
    public AudioClip[] sounds;
    private AudioSource source;
    public GameObject beatsLvl2GamePnl;
    private bool clickedOnce;

    [Range(0.1f, 0.7f)]  //value range header for volume
    public float volumeChangeMultiplyer = 0.4f;

    [Range(0.1f, 0.10f)] //value range header for the pritch
    public float pitchMultiplyer = 0.4f;

    //Lists to store the missed buttons, volume, and pitch variables for the level 2
    private BeatsResultsLvl2 beatsMissedSound;

    private List<BeatsResultsLvl2> beatsMissedSoundList;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        // get audio source
        source = GetComponent<AudioSource>();
        number = 0;
        beatsMissedSoundList = new List<BeatsResultsLvl2>();
    }

    //Reference: https://stackoverflow.com/questions/68889731/difference-between-whiletrue-in-coroutine-then-put-it-in-the-void-start-and/68889924#68889924
    private IEnumerator PlaySoundCoroutine()
    {
        if (sounds.Length > 0)
        {
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip soundToPlay = sounds[randomIndex];
            clickedOnce = false;
            beatsMissedSound = new BeatsResultsLvl2();

            //prevent the sounds from overlapping when another audio is playing
            if (source.isPlaying)
            {
                source.Stop();
            }

            source.volume = Random.Range(1 - volumeChangeMultiplyer, 1);    //generate the random volume based on the float range header
            source.pitch = Random.Range(1 - pitchMultiplyer, 1 + pitchMultiplyer);  //generate the random pitch based on the float range header
            source.PlayOneShot(soundToPlay);

            beatsMissedSound.SetPitchLvl2(source.pitch);
            beatsMissedSound.SetVolumeLvl2(source.volume);
            beatsMissedSound.SetMissedSoundLvl2(sounds[randomIndex].name);
        }
        yield return new WaitForSeconds(2f);                        //wait for 2 seconds before playing a sound again

        //score limit to swtitch scenes
        if (number == 10)
        {
            yield break;          //stop playing the sound when the score reaches 10
        }
    }

    public void BeatsBtnPressed()
    {
        if (number >= 10)
        {
            SceneManager.LoadScene("BeatsGameLevel3");  //name of level 3 scene
        }
        var clickedbtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedbtn.name == (beatsMissedSound.GetMissedSoundLvl2() + "Button") && number < 10 && clickedOnce == false && !source.isPlaying)
        {
            number++;
            clickedOnce = true;
            numberText.text = number.ToString();
            StartCoroutine(PlaySoundCoroutine());
        }
        else if (number < 10 && clickedOnce == false && !source.isPlaying)
        {
            clickedOnce = true;
            beatsMissedSoundList.Add(beatsMissedSound);
            StartCoroutine(PlaySoundCoroutine());
        }
    }

    public void BeatsPlayLvl2()
    {
        StartCoroutine(PlaySoundCoroutine()); //call method for IEnumerator
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.SetActive(false);
        beatsLvl2GamePnl.SetActive(true);
    }

    public List<BeatsResultsLvl2> GetBeatsResultsLvl2()
    {
        return beatsMissedSoundList;
    }

    public void ClearList()
    {
        beatsMissedSoundList.Clear();
    }
}