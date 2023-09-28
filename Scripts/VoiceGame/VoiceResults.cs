using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class VoiceResults
{
    [SerializeField]
    private string date;

    [SerializeField]
    private List<MissedSound> missedSounds = new();

    [SerializeField]
    private float timeToRespond;

    //adding the MissedSound objects to a priority list, items beeing added based on the object's int value (priority) which represents the number of ocurrences of that missed sound.
    //the objects are added to a priority list so that when read, the itmes will be displaied in order of their occurance
    public void AddMissedSound(int value, string key)
    {
        int iIndex = 0;
        MissedSound newMissedSound = new MissedSound { sound = key, value = value };

        iIndex = FindInsertPosition(value);

        if (iIndex > missedSounds.Count())
        {
            missedSounds.Add(newMissedSound);
        }
        else
        {
            missedSounds.Insert(iIndex, newMissedSound);
        }
    }

    public string GetDate()
    {
        return date;
    }

    public List<MissedSound> GetMissedSounds()
    {
        return missedSounds;
    }

    public float GetTimeToRespond()
    {
        return timeToRespond;
    }

    public void SetDate(string value)
    {
        date = value;
    }

    public void SetTimeToRespond(float value)
    {
        timeToRespond = value;
    }

    private int FindInsertPosition(int newKey)
    {
        MissedSound currSound;
        bool bFound = false;
        int iIndex = 0;
        while (iIndex < missedSounds.Count && !bFound)
        {
            currSound = missedSounds[iIndex];
            if (currSound.value > newKey)
            {
                iIndex = iIndex + 1;
            }
            else
            {
                bFound = true;
            }
        }
        return iIndex;
    }
}